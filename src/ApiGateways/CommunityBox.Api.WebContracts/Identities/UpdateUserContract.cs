namespace CommunityBox.Api.WebContracts.Identities
{
    public class UpdateUserContract
    {
        public string UserName { get; set; }
        
        public UserPersonalInformationContract UserPersonalInformation { get; set; }
        
        public AccountSettingContract AccountSetting { get; set; }
    }
}