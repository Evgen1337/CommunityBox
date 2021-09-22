using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBox.ChatService.Api.Proto;
using CommunityBox.Web.Mvc.Hubs;

namespace CommunityBox.Web.Mvc.Clients.Chat
{
    public interface IChatGrpcService
    {
        Task<IReadOnlyCollection<ChatPreviewModel>> GetChatsAsync(string userId);
        
        Task<long> GetChatIdAsync(string firstUserId, string secondUserId);

        Task SendMessageAsync(NewSingleMessageModel requestModel);

        UserConnection JoinAtMessenger(string userConnectionId, string userId,
            Action<JoinAtMessengerResponse> responseHandler);
    }
}