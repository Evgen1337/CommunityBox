namespace CommunityBox.Api.WebContracts.Auction
{
    public class SubscriberContract
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public long AuctionId { get; set; }
    }
}