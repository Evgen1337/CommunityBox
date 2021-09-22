using CommunityBox.ChatService.Domain.Entities;
using CommunityBox.Common.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityBox.ChatService.DAL.Configurations
{
    public class ChatUserConfiguration : EntityConfigurationBase<ChatUser>
    {
        public override void Configure(EntityTypeBuilder<ChatUser> builder)
        {
            builder
                .HasOne(x => x.Chat)
                .WithMany(x => x.ChatUsers)
                .HasForeignKey(f => f.ChatId);
            
            builder.HasIndex(u => u.UserId)
                .HasDatabaseName("ChatUser_UserIdIndex");
            
            base.Configure(builder);
        }
    }
}