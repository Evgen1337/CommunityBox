using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CommunityBox.AuctionService.Api.Application.Dtos;
using CommunityBox.AuctionService.Domain.Abstractions;
using CommunityBox.AuctionService.Domain.Entities;
using MediatR;

namespace CommunityBox.AuctionService.Api.Application.Commands
{
    public class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, AuctionDto>
    {
        private readonly IMapper _mapper;
        private readonly IAuctionContext _auctionContext;

        public CreateAuctionCommandHandler(IMapper mapper, IAuctionContext auctionContext)
        {
            _mapper = mapper;
            _auctionContext = auctionContext;
        }
        
        public async Task<AuctionDto> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Auction>(request.Auction);
            entity.CreationUtcDate = DateTime.UtcNow;
            
            var entityEntry = await _auctionContext.AddEntityAsync(entity);
            await _auctionContext.SaveChangesAsync();
            
            return _mapper.Map<AuctionDto>(entityEntry);
        }
    }
}