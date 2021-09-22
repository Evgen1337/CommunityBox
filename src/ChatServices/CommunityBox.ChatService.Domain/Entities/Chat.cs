using System.Collections.Generic;
using CommunityBox.Common.Core;

namespace CommunityBox.ChatService.Domain.Entities
{
    public class Chat : IEntity
    {
        public long Id { get; set; }
        
        public ICollection<ChatUser> ChatUsers { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}