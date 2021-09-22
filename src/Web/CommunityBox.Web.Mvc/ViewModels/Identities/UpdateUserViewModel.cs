namespace CommunityBox.Web.Mvc.ViewModels.Identities
{
    public class UpdateUserViewModel
    {
        public string UserName { get; set; }
        
        public UserPersonalInformationViewModel UserPersonalInformation { get; set; }
        
        public AccountSettingViewModel AccountSetting { get; set; }
    }
}