using CommunityBox.AuctionService.Domain.Entities;
using CommunityBox.Common.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityBox.AuctionService.DAL.Configurations
{
    public class AuctioneerConfiguration : EntityConfigurationBase<Auctioneer>
    {
        public override void Configure(EntityTypeBuilder<Auctioneer> builder)
        {
            builder
                .HasOne(x => x.Auction)
                .WithMany(x => x.Auctioneers)
                .HasForeignKey(x => x.AuctionId);
            
            builder.HasIndex(u => u.UserId)
                .HasDatabaseName("UserIdIndex");
            
            base.Configure(builder);
        }
    }
}