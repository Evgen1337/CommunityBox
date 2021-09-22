using System;

namespace CommunityBox.Web.Mvc.ViewModels.Chat
{
    public class ChatPreviewViewModel
    {
        public long ChatId { get; set; }
        
        public string LastMessageUserName { get; set; }
        
        public string LastMessageContent { get; set; }
        
        public DateTime LastMessageReceivedDate { get; set; }
    }
}