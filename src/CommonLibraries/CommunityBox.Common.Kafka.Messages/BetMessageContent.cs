namespace CommunityBox.Common.Kafka.Messages
{
    public class BetMessageContent
    {
        public BetAction Action { get; set; }

        public decimal Value { get; set; }

        public string UserId { get; set; }

        public string AuctionOwnerUserId { get; set; }
        
        public long AuctionId { get; set; }
    }
    
    public enum BetAction
    {
        None,
        Set,
        Remove
    }
}