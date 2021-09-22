using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBox.Api.WebContracts.Auction;
using CommunityBox.Api.WebContracts.Identities;

namespace CommunityBox.Web.Mvc.Clients.Gateway
{
    public interface IGatewayClient
    {
        Task<AuctionContract> CreateAuctionAsync(NewAuctionContract contract);
        Task<AuctionContract> UpdateAuctionAsync(UpdateAuctionContract contract, long id);
        Task DeleteAuctionAsync(long id);
        Task<AuctionContract> GetAuctionAsync(long id);
        Task<IReadOnlyCollection<AuctionContract>> GetAuctionsAsync(string userId);
        Task AddSubscriptionAsync(AddSubscriptionContract contract);
        Task RemoveSubscriptionAsync(long id);
        Task SetBetAsync(SetBetContract contract);
        Task RemoveBetAsync(long id);

        Task<AuthContract> AuthAsync(LogInContract contract);
        Task ValidateTokenAsync();
        Task<UserContract> GetUserAsync(string id);
        Task<UserContract> CreateUserAsync(NewUserContract contract);
        Task<UserContract> UpdateUserAsync(UpdateUserContract contract);
    }
}