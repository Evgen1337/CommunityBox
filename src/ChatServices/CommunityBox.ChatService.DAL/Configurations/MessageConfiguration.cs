using CommunityBox.ChatService.Domain.Entities;
using CommunityBox.Common.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityBox.ChatService.DAL.Configurations
{
    public class MessageConfiguration : EntityConfigurationBase<Message>
    {
        public override void Configure(EntityTypeBuilder<Message> builder)
        {
            builder
                .HasOne(x => x.Chat)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.ChatId);
            
            builder.HasIndex(u => u.UserId)
                .HasDatabaseName("Message_UserIdIndex");
            
            base.Configure(builder);
        }
    }
}