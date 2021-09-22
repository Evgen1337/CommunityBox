using CommunityBox.Common.Core;

namespace CommunityBox.AuctionService.Domain.Entities
{
    public class Subscriber : IEntity
    {
        public long Id { get; set; }
        
        public string UserId { get; set; }

        public Auction Auction { get; set; }
        public long AuctionId { get; set; }
    }
}