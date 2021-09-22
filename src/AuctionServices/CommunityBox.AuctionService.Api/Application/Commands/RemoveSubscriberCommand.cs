using MediatR;

namespace CommunityBox.AuctionService.Api.Application.Commands
{
    public class RemoveSubscriberCommand : IRequest
    {
        public string UserId { get; set; }

        public long AuctionId { get; set; }

        public RemoveSubscriberCommand(string userId, long auctionId)
        {
            UserId = userId;
            AuctionId = auctionId;
        }
    }
}