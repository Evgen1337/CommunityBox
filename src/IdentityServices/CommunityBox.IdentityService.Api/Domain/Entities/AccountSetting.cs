using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityBox.IdentityService.Api.Domain.Entities
{
    public class AccountSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public bool ShowPhone { get; set; } = true;
        
        public bool ShowEmail { get; set; } = true;
        
        public bool ShowBirthDay { get; set; } = true;
        
        public User User { get; set; }
        public string UserId { get; set; }
    }
}