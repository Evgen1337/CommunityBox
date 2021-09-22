using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityBox.Api.WebContracts.Auction;
using CommunityBox.Api.WebContracts.Identities;
using CommunityBox.Web.Mvc.Configurations;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Http;

namespace CommunityBox.Web.Mvc.Clients.Gateway
{
    public class GatewayClient : BaseGatewayClient, IGatewayClient
    {
        public GatewayClient(IFlurlClient flurlClient, NewtonsoftJsonSerializer jsonSerializer,
            GatewayClientConfig gatewayClientConfig, IHttpContextAccessor httpContextAccessor)
            : base(flurlClient, jsonSerializer, gatewayClientConfig, httpContextAccessor.HttpContext)
        {
        }

        public async Task<AuctionContract> CreateAuctionAsync(NewAuctionContract contract)
        {
            var response = await SendRequestAsync<AuctionContract, NewAuctionContract>(
                contract,
                ClientConfig.AuctionRoutes.CreateAuction,
                HttpMethod.Post,
                true
            );

            return response;
        }

        public async Task<AuctionContract> UpdateAuctionAsync(UpdateAuctionContract contract, long id)
        {
            var route = ClientConfig.AuctionRoutes.UpdateAuction.Replace("{id}", id.ToString());
            var response = await SendRequestAsync<AuctionContract, UpdateAuctionContract>(
                contract,
                route,
                HttpMethod.Put,
                true
            );

            return response;
        }

        public async Task DeleteAuctionAsync(long id)
        {
            var route = ClientConfig.AuctionRoutes.DeleteAuction.Replace("{id}", id.ToString());
            await SendRequestAsync(route, HttpMethod.Delete, true);
        }

        public async Task<AuctionContract> GetAuctionAsync(long id)
        {
            var route = ClientConfig.AuctionRoutes.GetAuction.Replace("{id}", id.ToString());
            var response = await SendRequestAsync<AuctionContract>(route, HttpMethod.Get, true);

            return response;
        }

        public async Task<IReadOnlyCollection<AuctionContract>> GetAuctionsAsync(string userId)
        {
            var route = ClientConfig.AuctionRoutes.GetListAuctions.Replace("{userId}", userId);
            var response = await SendRequestAsync<IReadOnlyCollection<AuctionContract>>(route, HttpMethod.Get);

            return response;
        }

        public async Task AddSubscriptionAsync(AddSubscriptionContract contract)
        {
            await SendRequestAsync(contract, ClientConfig.AuctionRoutes.AddSubscription, HttpMethod.Post, true);
        }

        public async Task RemoveSubscriptionAsync(long id)
        {
            var route = ClientConfig.AuctionRoutes.RemoveSubscription.Replace("{id}", id.ToString());
            await SendRequestAsync(route, HttpMethod.Delete, true);
        }

        public async Task SetBetAsync(SetBetContract contract)
        {
            await SendRequestAsync(contract, ClientConfig.AuctionRoutes.SetBet, HttpMethod.Post, true);
        }

        public async Task RemoveBetAsync(long id)
        {
            var route = ClientConfig.AuctionRoutes.RemoveBet.Replace("{id}", id.ToString());
            await SendRequestAsync(route, HttpMethod.Delete, true);
        }

        public async Task<AuthContract> AuthAsync(LogInContract contract)
        {
            var response = await SendRequestAsync<AuthContract, LogInContract>(
                contract,
                ClientConfig.IdentityRoutes.Auth,
                HttpMethod.Post
            );

            return response;
        }

        public async Task ValidateTokenAsync()
        {
            await SendRequestAsync(ClientConfig.IdentityRoutes.ValidateToken, HttpMethod.Get, true);
        }

        public async Task<UserContract> GetUserAsync(string id)
        {
            var route = ClientConfig.IdentityRoutes.GetUser.Replace("{id}", id);
            var response = await SendRequestAsync<UserContract>(route, HttpMethod.Get);

            return response;
        }

        public async Task<UserContract> CreateUserAsync(NewUserContract contract)
        {
            var response = await SendRequestAsync<UserContract, NewUserContract>(
                contract,
                ClientConfig.IdentityRoutes.CreateUser,
                HttpMethod.Post,
                true
            );

            return response;
        }

        public async Task<UserContract> UpdateUserAsync(UpdateUserContract contract)
        {
            var response = await SendRequestAsync<UserContract, UpdateUserContract>(
                contract,
                ClientConfig.IdentityRoutes.UpdateUser,
                HttpMethod.Put,
                true
            );

            return response;
        }
    }
}