using CommunityBox.Common.Core;

namespace CommunityBox.AuctionService.Domain.Entities
{
    public class Lot : IEntity
    {
        public long Id { get; set; }
        
        public string Name { get; set; }

        public string Comment { get; set; }

        public Auction Auction { get; set; }
        public long AuctionId { get; set; }
    }
}