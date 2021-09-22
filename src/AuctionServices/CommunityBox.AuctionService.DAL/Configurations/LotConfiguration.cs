using CommunityBox.AuctionService.Domain.Entities;
using CommunityBox.Common.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityBox.AuctionService.DAL.Configurations
{
    public class LotConfiguration : EntityConfigurationBase<Lot>
    {
        public override void Configure(EntityTypeBuilder<Lot> builder)
        {
            builder
                .HasOne(x => x.Auction)
                .WithOne(x => x.Lot)
                .HasForeignKey<Lot>(x => x.AuctionId);
            
            base.Configure(builder);
        }
    }
}