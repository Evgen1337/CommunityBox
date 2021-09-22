using System.Collections.Generic;

namespace CommunityBox.Web.Mvc.ViewModels.Chat
{
    public class ChatViewModel
    {
        public long Id { get; set; }
        
        public IReadOnlyCollection<MessageViewModel> Messages { get; set; }
    }
}