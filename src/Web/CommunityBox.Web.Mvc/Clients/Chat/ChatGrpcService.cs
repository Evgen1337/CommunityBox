using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityBox.ChatService.Api.Proto;
using CommunityBox.Web.Mvc.Hubs;
using Grpc.Core;
using Grpc.Net.Client;

namespace CommunityBox.Web.Mvc.Clients.Chat
{
    public class ChatGrpcService : IChatGrpcService
    {
        private readonly ChatGrpcServiceConfig _serviceConfig;

        public ChatGrpcService(ChatGrpcServiceConfig serviceConfig)
        {
            _serviceConfig = serviceConfig;
        }

        public async Task<IReadOnlyCollection<ChatPreviewModel>> GetChatsAsync(string userId)
        {
            var chats = await CallServiceAsync(async client =>
                await client.GetChatsAsync(new GetChatsRequest
                {
                    UserId = userId
                }));

            return chats.ChatPreviews.ToArray();
        }

        public async Task<long> GetChatIdAsync(string firstUserId, string secondUserId)
        {
            var chat = await CallServiceAsync(async client =>
                await client.GetChatIdAsync(new GetChatIdRequest
                {
                    FirstUserId = firstUserId,
                    SecondUserId = secondUserId
                }));

            return chat.Id;
        }

        public async Task SendMessageAsync(NewSingleMessageModel requestModel)
        {
            await CallServiceAsync(async client =>
                await client.SendSingleMessageAsync(new SendMessageRequest
                {
                    Message = requestModel
                }));
        }

        public UserConnection JoinAtMessenger(string userConnectionId, string userId,
            Action<JoinAtMessengerResponse> responseHandler)
        {
            var channel = GrpcChannel.ForAddress(_serviceConfig.BaseUrl);
            var client = new ChatServices.ChatServicesClient(channel);

            var userStream = client.JoinAtMessenger();

            var cancellationTokenSource = new CancellationTokenSource();
            var streamTask = new Task(async () =>
            {
                try
                {
                    while (await userStream.ResponseStream.MoveNext(cancellationTokenSource.Token))
                    {
                        var response = userStream.ResponseStream.Current;
                        responseHandler?.Invoke(response);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }, cancellationTokenSource.Token);

            var connection = new UserConnection
            {
                ConnectionId = userConnectionId,
                CsTokenSource = cancellationTokenSource,
                GrpcStreaming = userStream,
                GrpcChannel = channel,
                GrpcStreamingTask = streamTask,
                UserId = userId
            };

            return connection;
        }

        private async Task<TResponse> CallServiceAsync<TResponse>(
            Func<ChatServices.ChatServicesClient, Task<TResponse>> func)
        {
            using var channel = GrpcChannel.ForAddress(_serviceConfig.BaseUrl);
            var client = new ChatServices.ChatServicesClient(channel);

            return await func(client);
        }
    }
}