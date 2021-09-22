using System;

namespace CommunityBox.Web.Mvc.ViewModels.Identities
{
    public class UserPersonalInformationViewModel
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Bio { get; set; }

        public DateTime? BirthDay { get; set; }
    }
}