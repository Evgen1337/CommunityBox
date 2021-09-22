namespace CommunityBox.AuctionService.Api.Application.Dtos
{
    public class AuctioneerDto
    {
        public long? Id { get; set; }
        
        public long? AuctionId { get; set; }

        public string UserId { get; set; }

        public decimal Bet { get; set; }
    }
}