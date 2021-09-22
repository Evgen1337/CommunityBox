using System.Collections.Generic;
using System.Linq;

namespace CommunityBox.IdentityService.AuthService.Models
{
    public class PasswordValidateResponse
    {
        public PasswordValidateResponse()
        {
            Errors = new Dictionary<string, string>();
        }

        public IDictionary<string, string> Errors { get; set; }

        public bool Succeeded => !Errors.Any();
    }
}