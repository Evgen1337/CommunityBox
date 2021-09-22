namespace CommunityBox.AuctionService.Api.Application.Dtos
{
    public class BetDto
    {
        public string UserId { get; set; }

        public long AuctionId { get; set; }

        public decimal Value { get; set; }
    }
}