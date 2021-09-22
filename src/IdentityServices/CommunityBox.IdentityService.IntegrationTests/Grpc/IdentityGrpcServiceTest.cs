using System.Threading.Tasks;
using AutoFixture;
using CommunityBox.IdentityService.Api.Proto;
using CommunityBox.IdentityService.IntegrationTests.Infrastructure;
using Grpc.Core;
using Xunit;

namespace CommunityBox.IdentityService.IntegrationTests.Grpc
{
    public class IdentityGrpcServiceTest : TestApiBase<TestStartup>
    {
        private const string ApplicationUrl = "http://localhost:5110";

        [Fact]
        public async Task CreateUser_OK()
        {
            using var channel = PrepareTestAndGetGrpcChannel(ApplicationUrl);
            var client = new IdentityServices.IdentityServicesClient(channel);

            var createdEntity = await CreateUserAsync(client);
        }

        [Fact]
        public async Task UpdateUser_OK()
        {
            using var channel = PrepareTestAndGetGrpcChannel(ApplicationUrl);
            var client = new IdentityServices.IdentityServicesClient(channel);

            var newUserRequest = Fixture.Create<CreateUserRequest>();
            var createdEntity = await CreateUserAsync(client, newUserRequest);

            var authRequest = new AuthRequest
            {
                LogInModel = new LogInModel
                {
                    Email = newUserRequest.User.Email,
                    Password = newUserRequest.User.Password
                }
            };

            var authResponse = await AuthAsync(client, authRequest);

            var userFixture = Fixture
                .Build<UpdateUserModel>()
                .With(w => w.Id, createdEntity.User.Id)
                .Create();

            var headers = new Metadata
            {
                {"Authorization", $"Bearer {authResponse.Auth.Token}"}
            };

            var request = new UpdateUserRequest
            {
                User = userFixture
            };

            var updatedEntity = await client.UpdateUserAsync(request, headers);
        }

        [Fact]
        public async Task GetUser_OK()
        {
            using var channel = PrepareTestAndGetGrpcChannel(ApplicationUrl);
            var client = new IdentityServices.IdentityServicesClient(channel);

            var createdEntity = await CreateUserAsync(client);

            var request = new GetUserRequest
            {
                Id = createdEntity.User.Id
            };

            var retval = await client.GetUserAsync(request);
        }

        [Fact]
        public async Task Auth_OK()
        {
            using var channel = PrepareTestAndGetGrpcChannel(ApplicationUrl);
            var client = new IdentityServices.IdentityServicesClient(channel);

            var newUserRequest = Fixture.Create<CreateUserRequest>();
            var createdEntity = await CreateUserAsync(client, newUserRequest);

            var request = new AuthRequest
            {
                LogInModel = new LogInModel
                {
                    Email = createdEntity.User.Email,
                    Password = newUserRequest.User.Password
                }
            };

            var retval = await AuthAsync(client, request);
        }
        
        [Fact]
        public async Task ValidateToken_OK()
        {
            using var channel = PrepareTestAndGetGrpcChannel(ApplicationUrl);
            var client = new IdentityServices.IdentityServicesClient(channel);

            var newUserRequest = Fixture.Create<CreateUserRequest>();
            var createdEntity = await CreateUserAsync(client, newUserRequest);

            var authRequest = new AuthRequest
            {
                LogInModel = new LogInModel
                {
                    Email = createdEntity.User.Email,
                    Password = newUserRequest.User.Password
                }
            };

            var authResponse = await AuthAsync(client, authRequest);

            var request = new ValidateTokenRequest
            {
                Token = authResponse.Auth.Token
            };
            
            var headers = new Metadata
            {
                {"Authorization", $"Bearer {authResponse.Auth.Token}"}
            };

            var retval = await client.ValidateTokenAsync(request, headers);
            
            Assert.True(retval.IsValid);
        }

        private static async Task<AuthResponse> AuthAsync(IdentityServices.IdentityServicesClient client,
            AuthRequest request)
        {
            var response = await client.AuthAsync(request);
            return response;
        }

        private async Task<CreateUserResponse> CreateUserAsync(IdentityServices.IdentityServicesClient client)
        {
            var request = Fixture.Create<CreateUserRequest>();
            var response = await CreateUserAsync(client, request);
            return response;
        }

        private static async Task<CreateUserResponse> CreateUserAsync(IdentityServices.IdentityServicesClient client,
            CreateUserRequest createUserRequest)
        {
            var response = await client.CreateUserAsync(createUserRequest);
            return response;
        }
    }
}