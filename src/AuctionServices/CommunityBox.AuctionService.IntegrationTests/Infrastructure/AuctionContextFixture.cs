using System;
using CommunityBox.AuctionService.DAL;
using Microsoft.EntityFrameworkCore;

namespace CommunityBox.AuctionService.IntegrationTests.Infrastructure
{
    public class AuctionContextFixture : AuctionContext
    {
        public AuctionContextFixture(DbContextOptions<AuctionContext> options)
            : base(new DbContextOptionsBuilder<AuctionContext>()
                .UseInMemoryDatabase(databaseName: $"TestAuctionDb{DateTime.Now}")
                .Options
            )
        {
        }
    }
}