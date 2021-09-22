using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBox.ChatService.Api.Proto;
using CommunityBox.ChatService.Domain.Entities;
using CommunityBox.ChatService.Domain.Queries;
using Grpc.Core;

namespace CommunityBox.ChatService.Api.Services
{
    public interface IChatRepository
    {
        Task<IReadOnlyCollection<GetChatsQueryResult>> GetChatsAsync(string userId);
        Task<Chat> GetChatAsync(string firstUserId, string secondUserId);
        Task<Chat> GetChatAsync(long chatId);
        Task AddMessageAsync(Chat chat, Message message);
        IAsyncEnumerable<Message> GetChatMessagesAsync(long requestChatId);
        Task CreateOrAddOnChatMessageAsync(string senderUserId, string receiverUserId, string messageContent);
    }
}