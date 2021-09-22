using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityBox.AuctionService.Domain.Abstractions;
using CommunityBox.AuctionService.Domain.Entities;
using CommunityBox.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityBox.AuctionService.Api.Application.Commands
{
    public class DeleteAuctionCommandHandler : IRequestHandler<DeleteAuctionCommand>
    {
        private readonly IAuctionContext _auctionContext;

        public DeleteAuctionCommandHandler(IAuctionContext auctionContext)
        {
            _auctionContext = auctionContext;
        }
        
        public async Task<Unit> Handle(DeleteAuctionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _auctionContext.QueryEntity<Auction>()
                .Where(w => w.OwnerUserId == request.OwnerUserId && w.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (entity == null)
                throw new EntityNotFoundException();

            _auctionContext.RemoveEntity(entity);
            await _auctionContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}