using System.Threading.Tasks;
using CommunityBox.Api.WebGateway.Services.Abstractions;
using CommunityBox.Common.AuthHelpers;
using CommunityBox.IdentityService.Api.Proto;
using Grpc.Core;
using Microsoft.AspNetCore.Http;

namespace CommunityBox.Api.WebGateway.Services.Implementations
{
    public class IdentityGrpcService : BaseExternalService, IIdentityService
    {
        public IdentityGrpcService(IdentityServiceConfig serviceConfig)
            : base(serviceConfig)
        {
        }

        public async Task<AuthModel> AuthAsync(LogInModel requestModel)
        {
            var response = await CallServiceAsync(async channel =>
            {
                var client = new IdentityServices.IdentityServicesClient(channel);

                var request = new AuthRequest
                {
                    LogInModel = requestModel
                };

                return await client.AuthAsync(request);
            });

            return response.Auth;
        }

        public async Task<bool> ValidateTokenAsync(HttpRequest httpRequest)
        {
            var response = await CallServiceAsync(async channel =>
            {
                var client = new IdentityServices.IdentityServicesClient(channel);

                var request = new ValidateTokenRequest
                {
                    Token = httpRequest.GetAuthorizationToken()
                };

                return await client.ValidateTokenAsync(request);
            });

            return response.IsValid;
        }

        public async Task<UserModel> CreateUserAsync(NewUserModel requestModel)
        {
            var response = await CallServiceAsync(async channel =>
            {
                var client = new IdentityServices.IdentityServicesClient(channel);

                var request = new CreateUserRequest
                {
                    User = requestModel
                };

                return await client.CreateUserAsync(request);
            });

            return response.User;
        }

        public async Task<UserModel> GetUserAsync(string id)
        {
            var response = await CallServiceAsync(async channel =>
            {
                var client = new IdentityServices.IdentityServicesClient(channel);

                var request = new GetUserRequest
                {
                    Id = id
                };

                return await client.GetUserAsync(request);
            });

            return response.User;
        }

        public async Task<UserModel> UpdateUserAsync(UpdateUserModel requestModel, HttpRequest httpRequest)
        {
            var response = await CallServiceAsync(async channel =>
            {
                var client = new IdentityServices.IdentityServicesClient(channel);

                var request = new UpdateUserRequest
                {
                    User = requestModel
                };

                var headers = new Metadata
                {
                    {"Authorization", httpRequest.GetAuthorizationHeaderValue()}
                };

                return await client.UpdateUserAsync(request, headers);
            });

            return response.User;
        }
    }
}