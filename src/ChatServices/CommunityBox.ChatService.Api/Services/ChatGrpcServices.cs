using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityBox.ChatService.Api.Proto;
using CommunityBox.ChatService.Domain.Entities;
using CommunityBox.Common.Exceptions;
using CommunityBox.Common.GrpcBlocks.GrpcTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Connections;
using Empty = CommunityBox.ChatService.Api.Proto.Empty;

namespace CommunityBox.ChatService.Api.Services
{
    public class ChatGrpcServices : ChatServices.ChatServicesBase
    {
        private readonly IChatRepository _chatRepository;

        public ChatGrpcServices(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public override async Task<GetChatsResponse> GetChats(GetChatsRequest request, ServerCallContext context)
        {
            var chats = await _chatRepository.GetChatsAsync(request.UserId);

            var retval = new GetChatsResponse();
            foreach (var chatsQueryResult in chats)
            {
                retval.ChatPreviews.Add(new ChatPreviewModel
                {
                    ChatId = chatsQueryResult.ChatId,
                    LastMessageContent = chatsQueryResult.LastMessageContent,
                    LastMessageReceivedDate = chatsQueryResult.LastMessageReceivedDateUtc.ToTimestampWithKindUtc(),
                    LastMessageUserName = chatsQueryResult.LastMessageUserName
                });
            }

            return retval;
        }

        public override async Task<GetChatIdResponse> GetChatId(GetChatIdRequest request, ServerCallContext context)
        {
            var chat = await _chatRepository.GetChatAsync(request.FirstUserId, request.SecondUserId);
            return new GetChatIdResponse
            {
                Id = chat.Id
            };
        }

        public override async Task<Empty> SendSingleMessage(SendMessageRequest request, ServerCallContext context)
        {
            await _chatRepository.CreateOrAddOnChatMessageAsync(request.Message.UserId,
                request.Message.RecipientUserId, request.Message.Content);

            return new Empty();
        }

        public override async Task JoinAtMessenger(IAsyncStreamReader<JoinAtMessengerRequest> requestStream,
            IServerStreamWriter<JoinAtMessengerResponse> responseStream, ServerCallContext context)
        {
            try
            {
                var firstRequest = requestStream?.Current;
                await HandleRequestAsync(firstRequest, responseStream);

                while (await requestStream.MoveNext())
                {
                    var request = requestStream?.Current;
                    await HandleRequestAsync(request, responseStream);
                }
            }
            catch (Exception e) when (e.InnerException is ConnectionAbortedException)
            {
                Console.WriteLine(e);
            }
        }

        private async Task HandleRequestAsync(JoinAtMessengerRequest request,
            IServerStreamWriter<JoinAtMessengerResponse> responseStream)
        {
            if (request == null)
                return;

            switch (request.RequestType)
            {
                case JoinAtMessengerRequestType.None:
                    break;
                case JoinAtMessengerRequestType.GetMessages:
                    await HandleGetMessagesAsync(request, responseStream);
                    break;
                case JoinAtMessengerRequestType.SendMessage:
                    await HandleSendMessagesAsync(request, responseStream);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task HandleSendMessagesAsync(JoinAtMessengerRequest request,
            IServerStreamWriter<JoinAtMessengerResponse> responseStream)
        {
            var messagesResponse = new JoinAtMessengerResponse
            {
                RequestType = JoinAtMessengerRequestType.SendMessage,
                ChatId = request.ChatId
            };

            var chat = await _chatRepository.GetChatAsync(request.ChatId);

            if (chat == null)
                throw new EntityNotFoundException();

            var message = new Message
            {
                Chat = chat,
                Content = request.Message.Content,
                ReceivedDateUtc = DateTime.UtcNow,
                UserId = request.Message.UserId,
            };

            await _chatRepository.AddMessageAsync(chat, message);

            messagesResponse.Messages.Add(new MessageModel
            {
                ChatId = message.ChatId,
                Content = message.Content,
                UserId = message.UserId,
                ReceiverUserId = chat.ChatUsers.First(f => f.UserId != message.UserId).UserId,
                ReceivedDateUtc = message.ReceivedDateUtc.ToTimestampWithKindUtc(),
            });

            await responseStream.WriteAsync(messagesResponse);
        }

        private async Task HandleGetMessagesAsync(JoinAtMessengerRequest request,
            IServerStreamWriter<JoinAtMessengerResponse> responseStream)
        {
            var messages = _chatRepository.GetChatMessagesAsync(request.ChatId);

            var messagesResponse = new JoinAtMessengerResponse
            {
                RequestType = JoinAtMessengerRequestType.GetMessages,
                ChatId = request.ChatId
            };

            await foreach (var message in messages)
            {
                messagesResponse.Messages.Add(new MessageModel
                {
                    ChatId = message.ChatId,
                    Content = message.Content,
                    UserId = message.UserId,
                    ReceiverUserId = message.Chat.ChatUsers.First(f => f.UserId != message.UserId).UserId,
                    ReceivedDateUtc = message.ReceivedDateUtc.ToTimestampWithKindUtc(),
                });
            }

            await responseStream.WriteAsync(messagesResponse);
        }
    }
}