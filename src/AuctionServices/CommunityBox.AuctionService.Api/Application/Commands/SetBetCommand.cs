using MediatR;

namespace CommunityBox.AuctionService.Api.Application.Commands
{
    public class SetBetCommand : IRequest
    {
        public string UserId { get; set; }

        public long AuctionId { get; set; }
        
        public decimal Value { get; set; }

        public SetBetCommand(string userId, long auctionId, decimal value)
        {
            UserId = userId;
            AuctionId = auctionId;
            Value = value;
        }
    }
}