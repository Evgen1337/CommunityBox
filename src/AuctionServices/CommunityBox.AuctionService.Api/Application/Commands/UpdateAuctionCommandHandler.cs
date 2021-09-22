using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CommunityBox.AuctionService.Api.Application.Dtos;
using CommunityBox.AuctionService.Domain.Abstractions;
using CommunityBox.AuctionService.Domain.Entities;
using CommunityBox.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityBox.AuctionService.Api.Application.Commands
{
    public class UpdateAuctionCommandHandler : IRequestHandler<UpdateAuctionCommand, AuctionDto>
    {
        private readonly IMapper _mapper;
        private readonly IAuctionContext _auctionContext;

        public UpdateAuctionCommandHandler(IMapper mapper, IAuctionContext auctionContext)
        {
            _mapper = mapper;
            _auctionContext = auctionContext;
        }
        
        public async Task<AuctionDto> Handle(UpdateAuctionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _auctionContext.QueryEntity<Auction>()
                .Include(i => i.Lot)
                .Where(w => w.OwnerUserId == request.Auction.OwnerUserId && w.Id == request.Auction.Id)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (entity == null)
                throw new EntityNotFoundException();

            entity.Duration = request.Auction.Duration;
            entity.UpdateUtcDate = DateTime.UtcNow;
            entity.Lot.Comment = request.Auction.Lot.Comment;
            entity.Lot.Name = request.Auction.Lot.Name;

            var entityEntry = _auctionContext.UpdateEntity(entity);
            await _auctionContext.SaveChangesAsync();

            return _mapper.Map<AuctionDto>(entityEntry);
        }
    }
}