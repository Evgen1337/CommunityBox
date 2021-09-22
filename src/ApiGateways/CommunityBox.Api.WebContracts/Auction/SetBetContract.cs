namespace CommunityBox.Api.WebContracts.Auction
{
    public class SetBetContract
    {
        public decimal Value { get; set; }
        
        public long AuctionId { get; set; }
    }
}