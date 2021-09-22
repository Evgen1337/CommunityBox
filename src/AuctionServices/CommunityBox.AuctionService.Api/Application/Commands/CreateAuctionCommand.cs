using CommunityBox.AuctionService.Api.Application.Dtos;
using MediatR;

namespace CommunityBox.AuctionService.Api.Application.Commands
{
    public class CreateAuctionCommand : IRequest<AuctionDto>
    {
        public NewAuctionDto Auction { get; }

        public CreateAuctionCommand(NewAuctionDto auction)
        {
            Auction = auction;
        }
    }
}