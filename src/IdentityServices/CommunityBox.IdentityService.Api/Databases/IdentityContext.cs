using CommunityBox.IdentityService.Api.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CommunityBox.IdentityService.Api.Databases
{
    public class IdentityContext : IdentityDbContext<User>
    {
        public DbSet<AccountSetting> AccountSettings { get; set; }

        public DbSet<UserPersonalInformation> UserPersonalInformation { get; set; }

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.PhoneNumber)
                    .HasDatabaseName("PhoneNumberIndex")
                    .IsUnique();
                
                entity.HasIndex(u => u.NormalizedEmail)
                    .HasDatabaseName("NormalizedEmailIndex")
                    .IsUnique();
            });
            
            builder.Entity<AccountSetting>(entity =>
            {
                entity.HasOne(x => x.User)
                    .WithOne(x => x.AccountSetting)
                    .HasForeignKey<AccountSetting>(x => x.UserId);
            });
            
            builder.Entity<UserPersonalInformation>(entity =>
            {
                entity.HasOne(x => x.User)
                    .WithOne(x => x.UserPersonalInformation)
                    .HasForeignKey<UserPersonalInformation>(x => x.UserId);
            });

            base.OnModelCreating(builder);
        }
    }
}