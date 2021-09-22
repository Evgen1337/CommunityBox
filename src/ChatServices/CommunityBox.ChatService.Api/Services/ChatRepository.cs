using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityBox.ChatService.Api.Clients;
using CommunityBox.ChatService.Domain.Abstractions;
using CommunityBox.ChatService.Domain.Entities;
using CommunityBox.ChatService.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CommunityBox.ChatService.Api.Services
{
    public class ChatRepository : IChatRepository
    {
        private readonly IChatContext _chatContext;
        private readonly IIdentityClient _identityClient;

        public ChatRepository(IChatContext chatContext, IIdentityClient identityClient)
        {
            _chatContext = chatContext;
            _identityClient = identityClient;
        }

        public async Task<IReadOnlyCollection<GetChatsQueryResult>> GetChatsAsync(string userId)
        {
            var param = new NpgsqlParameter("@userId", userId);
            var rawQuery = _chatContext.RawQueryAsync<GetChatsQueryResult>(GetChatsQuery, param);

            var chatsQueryResults = await rawQuery.ToArrayAsync();
            return chatsQueryResults;
        }

        public async Task<Chat> GetChatAsync(string firstUserId, string secondUserId)
        {
            var chat = await _chatContext.QueryEntity<Chat>()
                .Include(i => i.Messages)
                .Include(i => i.ChatUsers)
                .Where(w =>
                    w.ChatUsers.Any(a => a.UserId == firstUserId) &&
                    w.ChatUsers.Any(a => a.UserId == secondUserId)
                ).FirstOrDefaultAsync();

            return chat;
        }

        public async Task<Chat> GetChatAsync(long chatId)
        {
            var chat = await _chatContext.QueryEntity<Chat>()
                .Include(i => i.Messages)
                .Include(i => i.ChatUsers)
                .Where(w => w.Id == chatId)
                .FirstOrDefaultAsync();

            return chat;
        }

        public async Task AddMessageAsync(Chat chat, Message message)
        {
            chat.Messages.Add(message);
            await _chatContext.SaveChangesAsync();
        }

        public IAsyncEnumerable<Message> GetChatMessagesAsync(long requestChatId)
        {
            var messages = _chatContext.QueryEntity<Message>()
                .Include(i => i.Chat)
                .ThenInclude(i => i.ChatUsers)
                .Where(w => w.ChatId == requestChatId)
                .AsNoTracking()
                .AsAsyncEnumerable();

            return messages;
        }

        public async Task CreateOrAddOnChatMessageAsync(string senderUserId, string receiverUserId,
            string messageContent)
        {
            var chat = await GetChatAsync(senderUserId, receiverUserId);

            var message = new Message
            {
                Content = messageContent,
                UserId = senderUserId,
                ReceivedDateUtc = DateTime.UtcNow,
                Chat = chat
            };

            if (chat != null)
            {
                await AddMessageAsync(chat, message);
                return;
            }

            var firstUser = await GetChatUserAsync(senderUserId);
            var secondUser = await GetChatUserAsync(receiverUserId);

            var newChat = new Chat
            {
                ChatUsers = new[]
                {
                    firstUser,
                    secondUser
                },
                Messages = new[]
                {
                    message
                }
            };

            await CreateNewChatAsync(newChat);
        }

        private async Task<ChatUser> GetChatUserAsync(string userId)
        {
            var user = await _identityClient.GetUserAsync(userId);

            var chatUser = new ChatUser
            {
                UserId = userId,
                UserName = user.UserName
            };

            return chatUser;
        }

        private async Task CreateNewChatAsync(Chat newChat)
        {
            await _chatContext.AddEntityAsync(newChat);
            await _chatContext.SaveChangesAsync();
        }

        private const string GetChatsQuery = @"
        select distinct
        	lastMessages.""ChatId"",
        	""ReceivedDateUtc"" as ""LastMessageReceivedDateUtc"",
        	""Content"" as ""LastMessageContent"",
        	""UserName"" as ""LastMessageUserName""
        from
            (
            select
        	    m.""UserId"",
        	    ""ReceivedDateUtc"",
        	    ""Content"",
        	    m.""ChatId"",
        	    rank() over (partition by m.""ChatId""
            order by
        	    m.""ReceivedDateUtc"" desc) as dest_rank
            from
        	    ""Messages"" as m
            where
        	    exists (
        	    select
        		    1
        	    from
        		    ""ChatUsers"" cu
        	    where
        		    cu.""UserId"" = @userId
        		    and cu.""ChatId"" = m.""ChatId"" 
            )
        ) as lastMessages
        inner join ""ChatUsers"" cu on
        	cu.""ChatId"" = lastMessages.""ChatId""
        	and cu.""UserId"" = lastMessages.""UserId""
        where
        	dest_rank = 1
        ";
    }
}