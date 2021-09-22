using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using CommunityBox.ChatService.Api.Proto;
using CommunityBox.Common.Core;
using CommunityBox.Common.Core.SystemChat;
using CommunityBox.Web.Mvc.Clients.Chat;
using CommunityBox.Web.Mvc.Controllers;
using CommunityBox.Web.Mvc.ViewModels.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CommunityBox.Web.Mvc.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatGrpcService _chatGrpcClient;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatHub(IChatGrpcService chatGrpcClient, IHubContext<ChatHub> hubContext)
        {
            _chatGrpcClient = chatGrpcClient;
            _hubContext = hubContext;
        }

        public async Task JoinAtChat(string targetChatId)
        {
            var chatId = long.Parse(targetChatId);
            var connId = Context.ConnectionId;
            
            await GetMessagesRequestAsync(chatId, connId);
        }

        public async Task SendMessage(string targetChatId, string content)
        {
            var chatId = long.Parse(targetChatId);
            var userId = GetUserId();
            
            await SendMessageRequestAsync(chatId, content, userId);
        }

        public override async Task OnConnectedAsync()
        {
            var userId = GetUserId();
            var connId = Context.ConnectionId;

            var userConnection = _chatGrpcClient.JoinAtMessenger(connId, userId,
                async (response) => { await HandleJoinAtChatAsync(response, connId); });

            ChatHubConnectionsRepository.Add(connId, userConnection);

            userConnection.GrpcStreamingTask.Start();

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connId = Context.ConnectionId;
            var connection = ChatHubConnectionsRepository.GetByCollectionId(connId);

            if (connection is null)
                return;

            connection.Dispose();

            ChatHubConnectionsRepository.Remove(connId);

            await base.OnDisconnectedAsync(exception);
        }

        private async Task HandleJoinAtChatAsync(JoinAtMessengerResponse response, string connId)
        {
            var responseMessages = response.Messages;

            var messageViewModels = responseMessages.Select(responseMessage => new MessageViewModel
            {
                UserId = responseMessage.UserId,
                Content = responseMessage.Content,
                ReceivedDate = responseMessage.ReceivedDateUtc.ToDateTime().ToString("dd.MM.yy hh:mm")
            }).ToArray();

            var retval = new ChatViewModel
            {
                Id = response.ChatId,
                Messages = messageViewModels
            };

            var client = _hubContext.Clients.Client(connId);
            await client.SendAsync("HandleResponseMessages", retval);

            if (response.RequestType != JoinAtMessengerRequestType.SendMessage)
                return;

            var receiverUserId = response.Messages.FirstOrDefault()?.ReceiverUserId;

            if (receiverUserId == null)
                return;

            var receiverConnections = ChatHubConnectionsRepository.GetByUserId(receiverUserId);

            if (!receiverConnections.Any())
                return;

            var tasks = receiverConnections.Select(receiverConnection =>
                Task.Run(async () =>
                    {
                        var receiverClient = _hubContext.Clients.Client(receiverConnection.ConnectionId);
                        await receiverClient.SendAsync("HandleResponseMessages", retval);
                    }
                )
            );

            await Task.WhenAll(tasks);
        }

        private static async Task SendMessageRequestAsync(long chatId, string content, string userId)
        {
            var connections = ChatHubConnectionsRepository.GetByUserId(userId);

            if (!connections.Any())
                return;

            var tasks = connections.Select(connection =>
                Task.Run(async () =>
                    await connection.GrpcStreaming.RequestStream.WriteAsync(new JoinAtMessengerRequest
                    {
                        RequestType = JoinAtMessengerRequestType.SendMessage,
                        ChatId = chatId,
                        Message = new NewMessageModel
                        {
                            UserId = userId,
                            Content = content
                        }
                    })
                )
            );

            await Task.WhenAll(tasks);
        }

        private static async Task GetMessagesRequestAsync(long chatId, string connectionId)
        {
            var connection = ChatHubConnectionsRepository.GetByCollectionId(connectionId);

            if (connection == null)
                return;

            await connection.GrpcStreaming.RequestStream.WriteAsync(new JoinAtMessengerRequest
            {
                RequestType = JoinAtMessengerRequestType.GetMessages,
                ChatId = chatId
            });
        }

        private string GetUserId() =>
            Context.User?.Claims.FirstOrDefault(f => f.Type == "uid")?.Value;
    }
}