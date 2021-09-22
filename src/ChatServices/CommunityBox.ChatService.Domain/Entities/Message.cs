using System;
using CommunityBox.Common.Core;

namespace CommunityBox.ChatService.Domain.Entities
{
    public class Message : IEntity
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public string Content { get; set; }

        public DateTime ReceivedDateUtc { get; set; }

        public Chat Chat { get; set; }
        public long ChatId { get; set; }
    }
}