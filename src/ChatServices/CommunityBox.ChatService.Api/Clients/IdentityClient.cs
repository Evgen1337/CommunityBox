using System.Threading.Tasks;
using CommunityBox.IdentityService.Api.Proto;
using Grpc.Net.Client;

namespace CommunityBox.ChatService.Api.Clients
{
    public class IdentityClient : IIdentityClient
    {
        private readonly IdentityServiceConfig _config;

        public IdentityClient(IdentityServiceConfig config)
        {
            _config = config;
        }
        
        public async Task<UserModel> GetUserAsync(string userId)
        {
            using var channel = GrpcChannel.ForAddress(_config.ApplicationUrl);
            var client = new IdentityServices.IdentityServicesClient(channel);

            var response = await client.GetUserAsync(new GetUserRequest
            {
                Id = userId
            });

            return response.User;
        }
    }
}