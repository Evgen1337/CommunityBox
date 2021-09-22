using System;

namespace CommunityBox.Web.Mvc.ViewModels.Identities
{
    public class UserViewModel
    {
        public string Id { get; set; }
        
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public DateTime CreationUtcDate { get; set; }
        
        public DateTime? UpdateUtcDate { get; set; }

        public UserPersonalInformationViewModel UserPersonalInformation { get; set; }

        public bool IsMyProfile { get; set; }
    }
}