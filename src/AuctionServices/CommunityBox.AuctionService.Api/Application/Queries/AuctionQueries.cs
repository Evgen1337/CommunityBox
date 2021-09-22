using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommunityBox.AuctionService.Api.Application.Dtos;
using CommunityBox.AuctionService.Domain.Abstractions;
using CommunityBox.AuctionService.Domain.Entities;
using CommunityBox.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CommunityBox.AuctionService.Api.Application.Queries
{
    public class AuctionQueries : IAuctionQueries
    {
        private readonly IAuctionContext _auctionContext;
        private readonly IMapper _mapper;

        public AuctionQueries(IAuctionContext auctionContext, IMapper mapper)
        {
            _auctionContext = auctionContext;
            _mapper = mapper;
        }

        public async Task<AuctionDto> GetAuctionAsync(long id)
        {
            var entity = await _auctionContext.QueryEntity<Auction>()
                .Include(i => i.Lot)
                .Include(i => i.Auctioneers)
                .Include(i => i.Subscribers)
                .Where(w => w.Id == id)
                .FirstOrDefaultAsync();

            if (entity == null)
                throw new EntityNotFoundException();

            var retval = _mapper.Map<AuctionDto>(entity);
            return retval;
        }

        public async Task<IReadOnlyCollection<AuctionDto>> GetAuctionListAsync(string userId)
        {
            var entities = await _auctionContext.QueryEntity<Auction>()
                .Include(i => i.Lot)
                .Include(i => i.Auctioneers)
                .Include(i => i.Subscribers)
                .Where(w => w.OwnerUserId == userId)
                .ToArrayAsync();

            var retval = _mapper.Map<IReadOnlyCollection<AuctionDto>>(entities);
            return retval;
        }
    }
}