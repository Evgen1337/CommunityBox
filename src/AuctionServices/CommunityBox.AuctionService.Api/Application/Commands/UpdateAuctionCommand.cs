using CommunityBox.AuctionService.Api.Application.Dtos;
using MediatR;

namespace CommunityBox.AuctionService.Api.Application.Commands
{
    public class UpdateAuctionCommand : IRequest<AuctionDto>
    {
        public UpdateAuctionDto Auction { get; }

        public UpdateAuctionCommand(UpdateAuctionDto auction)
        {
            Auction = auction;
        }
    }
}