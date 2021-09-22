namespace CommunityBox.Common.Kafka.Messages
{
    public class SubscriberMessageContent
    {
        public string UserId { get; set; }

        public string AuctionOwnerUserId { get; set; }

        public long AuctionId { get; set; }

        public SubscriberAction Action { get; set; }
    }

    public enum SubscriberAction
    {
        None,
        Sub,
        Unsub
    }
}