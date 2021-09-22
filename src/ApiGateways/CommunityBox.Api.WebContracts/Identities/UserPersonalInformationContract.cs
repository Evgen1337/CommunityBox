using System;

namespace CommunityBox.Api.WebContracts.Identities
{
    public class UserPersonalInformationContract
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Bio { get; set; }

        public DateTime? BirthDay { get; set; }
    }
}