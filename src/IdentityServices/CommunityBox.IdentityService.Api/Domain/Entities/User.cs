using System;
using Microsoft.AspNetCore.Identity;

namespace CommunityBox.IdentityService.Api.Domain.Entities
{
    public class User : IdentityUser
    {
        public AccountSetting AccountSetting { get; set; }
        
        public UserPersonalInformation UserPersonalInformation { get; set; }

        public DateTime CreationUtcDate { get; set; }

        public DateTime? UpdateUtcDate { get; set; }
    }
}