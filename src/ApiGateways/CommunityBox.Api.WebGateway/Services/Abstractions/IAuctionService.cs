using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBox.AuctionService.Api.Proto;

namespace CommunityBox.Api.WebGateway.Services.Abstractions
{
    public interface IAuctionService
    {
        Task<AuctionModel> CreateAsync(NewAuctionModel requestModel);
        Task<AuctionModel> UpdateAsync(UpdateAuctionModel requestModel);
        Task DeleteAsync(long id, string userId);
        Task<AuctionModel> GetAsync(long id);
        Task<IReadOnlyCollection<AuctionModel>> GetListAsync(string userId);
        Task AddSubscriberAsync(string userId, long id);
        Task RemoveSubscriberAsync(string userId, long id);
        Task SetBetAsync(string userId, long id, decimal value);
        Task RemoveBetAsync(string userId, long id);
    }
}