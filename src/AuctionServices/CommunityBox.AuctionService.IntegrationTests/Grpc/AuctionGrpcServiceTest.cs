using System;
using System.Threading.Tasks;
using AutoFixture;
using CommunityBox.AuctionService.Api.Proto;
using CommunityBox.AuctionService.IntegrationTests.Infrastructure;
using Google.Protobuf.WellKnownTypes;
using Xunit;

namespace CommunityBox.AuctionService.IntegrationTests.Grpc
{
    public class AuctionGrpcServiceTest : TestApiBase<TestStartup>
    {
        private const string ApplicationUrl = "http://localhost:5100";

        [Fact]
        public async Task CreateAuction_OK()
        {
            using var channel = PrepareTestAndGetGrpcChannel(ApplicationUrl);
            var client = new AuctionServices.AuctionServicesClient(channel);

            var createdEntity = await CreateAuctionAsync(client);
        }

        [Fact]
        public async Task DeleteAuction_OK()
        {
            using var channel = PrepareTestAndGetGrpcChannel(ApplicationUrl);
            var client = new AuctionServices.AuctionServicesClient(channel);

            var createdEntity = await CreateAuctionAsync(client);

            var request = new DeleteAuctionRequest
            {
                Id = createdEntity.Auction.Id,
                OwnerUserId = createdEntity.Auction.OwnerUserId
            };

            await client.DeleteAsync(request);
        }

        [Fact]
        public async Task UpdateAuction_OK()
        {
            using var channel = PrepareTestAndGetGrpcChannel(ApplicationUrl);
            var client = new AuctionServices.AuctionServicesClient(channel);

            var createdEntity = await CreateAuctionAsync(client);

            createdEntity.Auction.Lot.Name = "Name 123";
            createdEntity.Auction.Lot.Comment = "Comment 123";

            var request = new UpdateAuctionRequest
            {
                Auction = new UpdateAuctionModel
                {
                    Id = createdEntity.Auction.Id,
                    OwnerUserId = createdEntity.Auction.OwnerUserId,
                    Duration = Duration.FromTimeSpan(new TimeSpan(12, 12, 12)),
                    Lot = createdEntity.Auction.Lot
                }
            };

            var updatedEntity = await client.UpdateAsync(request);
        }

        [Fact]
        public async Task GetAuction_OK()
        {
            using var channel = PrepareTestAndGetGrpcChannel(ApplicationUrl);
            var client = new AuctionServices.AuctionServicesClient(channel);

            var createdEntity = await CreateAuctionAsync(client);

            var request = new GetAuctionRequest
            {
                Id = createdEntity.Auction.Id
            };

            var retval = await client.GetAsync(request);
        }

        [Fact]
        public async Task GetListAuction_OK()
        {
            using var channel = PrepareTestAndGetGrpcChannel(ApplicationUrl);
            var client = new AuctionServices.AuctionServicesClient(channel);

            var createdEntities = new[]
            {
                await CreateAuctionAsync(client),
                await CreateAuctionAsync(client),
                await CreateAuctionAsync(client)
            };

            var request = new GetAuctionListRequest();
            var retval = await client.GetListAsync(request);
        }

        [Fact]
        public async Task SetBet_OK()
        {
            using var channel = PrepareTestAndGetGrpcChannel(ApplicationUrl);
            var client = new AuctionServices.AuctionServicesClient(channel);

            var createdEntity = await CreateAuctionAsync(client);

            await client.SetBetAsync(new SetBetRequest
            {
                AuctionId = createdEntity.Auction.Id,
                UserId = "userId",
                Value = new DecimalValue
                {
                    Nanos = 12345
                }
            });
        }

        private async Task<CreateAuctionResponse> CreateAuctionAsync(AuctionServices.AuctionServicesClient client)
        {
            var request = Fixture.Create<CreateAuctionRequest>();
            var response = await client.CreateAsync(request);
            return response;
        }
    }
}