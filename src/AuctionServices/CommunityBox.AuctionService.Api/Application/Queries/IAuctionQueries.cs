using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBox.AuctionService.Api.Application.Dtos;

namespace CommunityBox.AuctionService.Api.Application.Queries
{
    public interface IAuctionQueries
    {
        Task<AuctionDto> GetAuctionAsync(long id);
        
        Task<IReadOnlyCollection<AuctionDto>> GetAuctionListAsync(string userId);
    }
}