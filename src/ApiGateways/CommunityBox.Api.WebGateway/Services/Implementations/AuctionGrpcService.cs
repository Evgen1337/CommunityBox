using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBox.Api.WebGateway.Protos;
using CommunityBox.Api.WebGateway.Services.Abstractions;
using CommunityBox.AuctionService.Api.Proto;

namespace CommunityBox.Api.WebGateway.Services.Implementations
{
    public class AuctionGrpcService : BaseExternalService, IAuctionService
    {
        public AuctionGrpcService(AuctionServiceConfig externalServiceConfig)
            : base(externalServiceConfig)
        {
        }

        public async Task<AuctionModel> CreateAsync(NewAuctionModel requestModel)
        {
            var response = await CallServiceAsync(async channel =>
            {
                var client = new AuctionServices.AuctionServicesClient(channel);

                var request = new CreateAuctionRequest
                {
                    Auction = requestModel
                };

                return await client.CreateAsync(request);
            });

            return response.Auction;
        }

        public async Task<AuctionModel> UpdateAsync(UpdateAuctionModel requestModel)
        {
            var response = await CallServiceAsync(async channel =>
            {
                var client = new AuctionServices.AuctionServicesClient(channel);

                var request = new UpdateAuctionRequest
                {
                    Auction = requestModel
                };

                return await client.UpdateAsync(request);
            });

            return response.Auction;
        }

        public async Task DeleteAsync(long id, string userId)
        {
            await CallServiceAsync(async channel =>
            {
                var client = new AuctionServices.AuctionServicesClient(channel);

                var request = new DeleteAuctionRequest
                {
                    Id = id,
                    OwnerUserId = userId
                };

                return await client.DeleteAsync(request);
            });
        }

        public async Task<AuctionModel> GetAsync(long id)
        {
            var response = await CallServiceAsync(async channel =>
            {
                var client = new AuctionServices.AuctionServicesClient(channel);

                var request = new GetAuctionRequest
                {
                    Id = id
                };

                return await client.GetAsync(request);
            });

            return response.Auction;
        }

        public async Task<IReadOnlyCollection<AuctionModel>> GetListAsync(string userId)
        {
            var response = await CallServiceAsync(async channel =>
            {
                var client = new AuctionServices.AuctionServicesClient(channel);

                var request = new GetAuctionListRequest
                {
                    UserId = userId
                };

                return await client.GetListAsync(request);
            });

            return response.Auctions;
        }

        public async Task AddSubscriberAsync(string userId, long id)
        {
            await CallServiceAsync(async channel =>
            {
                var client = new AuctionServices.AuctionServicesClient(channel);

                var request = new AddSubscriberRequest
                {
                    AuctionId = id,
                    UserId = userId
                };

                return await client.AddSubscriberAsync(request);
            });
        }

        public async Task RemoveSubscriberAsync(string userId, long id)
        {
            await CallServiceAsync(async channel =>
            {
                var client = new AuctionServices.AuctionServicesClient(channel);

                var request = new RemoveSubscriberRequest
                {
                    AuctionId = id,
                    UserId = userId
                };

                return await client.RemoveSubscriberAsync(request);
            });
        }

        public async Task SetBetAsync(string userId, long id, decimal value)
        {
            await CallServiceAsync(async channel =>
            {
                var client = new AuctionServices.AuctionServicesClient(channel);

                var request = new SetBetRequest
                {
                    AuctionId = id,
                    UserId = userId,
                    Value = value.ToDecimalValue()
                };

                return await client.SetBetAsync(request);
            });
        }

        public async Task RemoveBetAsync(string userId, long id)
        {
            await CallServiceAsync(async channel =>
            {
                var client = new AuctionServices.AuctionServicesClient(channel);

                var request = new RemoveBetRequest
                {
                    AuctionId = id,
                    UserId = userId
                };

                return await client.RemoveBetAsync(request);
            });
        }
    }
}