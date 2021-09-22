using MediatR;

namespace CommunityBox.AuctionService.Api.Application.Commands
{
    public class DeleteAuctionCommand : IRequest
    {
        public long Id { get; }

        public string OwnerUserId { get; }

        public DeleteAuctionCommand(long id, string ownerUserId)
        {
            Id = id;
            OwnerUserId = ownerUserId;
        }
    }
}