using MediatR;

namespace CommunityBox.AuctionService.Api.Application.Commands
{
    public class AddSubscriberCommand : IRequest
    {
        public string UserId { get; set; }

        public long AuctionId { get; set; }

        public AddSubscriberCommand(string userId, long auctionId)
        {
            UserId = userId;
            AuctionId = auctionId;
        }
    }
}