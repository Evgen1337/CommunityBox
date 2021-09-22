namespace CommunityBox.AuctionService.Api.Application.Dtos
{
    public class SubscriberDto
    {
        public long Id { get; set; }

        public string UserId { get; set; }
        
        public long AuctionId { get; set; }
    }
}