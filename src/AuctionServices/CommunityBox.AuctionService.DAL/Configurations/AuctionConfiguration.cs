using CommunityBox.AuctionService.Domain.Entities;
using CommunityBox.Common.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityBox.AuctionService.DAL.Configurations
{
    public class AuctionConfiguration : EntityConfigurationBase<Auction>
    {
        public override void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.HasIndex(u => u.OwnerUserId)
                .HasDatabaseName("OwnerUserIdIndex");
            
            base.Configure(builder);
        }
    }
}