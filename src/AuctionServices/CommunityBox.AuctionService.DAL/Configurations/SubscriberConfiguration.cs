using CommunityBox.AuctionService.Domain.Entities;
using CommunityBox.Common.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityBox.AuctionService.DAL.Configurations
{
    public class SubscriberConfiguration : EntityConfigurationBase<Subscriber>
    {
        public override void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            builder
                .HasOne(x => x.Auction)
                .WithMany(x => x.Subscribers)
                .HasForeignKey(x => x.AuctionId);
            
            builder.HasIndex(u => u.UserId)
                .HasDatabaseName("UserIdIndex");
            
            base.Configure(builder);
        }
    }
}