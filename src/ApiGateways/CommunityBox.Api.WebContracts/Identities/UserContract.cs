using System;

namespace CommunityBox.Api.WebContracts.Identities
{
    public class UserContract
    {
        public string Id { get; set; }
        
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public DateTime CreationUtcDate { get; set; }
        
        public DateTime? UpdateUtcDate { get; set; }
        
        public UserPersonalInformationContract UserPersonalInformation { get; set; }
        
        public AccountSettingContract AccountSetting { get; set; }
    }
}