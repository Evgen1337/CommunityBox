using MediatR;

namespace CommunityBox.AuctionService.Api.Application.Commands
{
    public class RemoveBetCommand : IRequest
    {
        public string UserId { get; set; }

        public long AuctionId { get; set; }

        public RemoveBetCommand(string userId, long auctionId)
        {
            UserId = userId;
            AuctionId = auctionId;
        }
    }
}