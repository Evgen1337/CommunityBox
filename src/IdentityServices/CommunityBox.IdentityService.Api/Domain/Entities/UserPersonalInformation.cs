using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityBox.IdentityService.Api.Domain.Entities
{
    public class UserPersonalInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Bio { get; set; }

        public DateTime? BirthDay { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
    }
}