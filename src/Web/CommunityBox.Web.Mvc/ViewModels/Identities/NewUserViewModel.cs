namespace CommunityBox.Web.Mvc.ViewModels.Identities
{
    public class NewUserViewModel
    {
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Password { get; set; }
        
        public UserPersonalInformationViewModel UserPersonalInformation { get; set; }
    }
}