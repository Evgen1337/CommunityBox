namespace CommunityBox.Api.WebContracts.Identities
{
    public class NewUserContract
    {
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Password { get; set; }
        
        public UserPersonalInformationContract UserPersonalInformation { get; set; }
    }
}