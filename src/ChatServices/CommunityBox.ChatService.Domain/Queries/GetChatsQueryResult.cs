using System;
using CommunityBox.Common.Core;

namespace CommunityBox.ChatService.Domain.Queries
{
    public class GetChatsQueryResult : IRawQueryModel
    {
        public long ChatId { get; set; }

        public string LastMessageUserName { get; set; }

        public string LastMessageContent { get; set; }

        public DateTime LastMessageReceivedDateUtc { get; set; }
    }
}