using CommunityBox.Common.Core;

namespace CommunityBox.ChatService.Domain.Entities
{
    public class ChatUser : IEntity
    {
        public long Id { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        
        public Chat Chat { get; set; }
        public long ChatId { get; set; }
    }
}