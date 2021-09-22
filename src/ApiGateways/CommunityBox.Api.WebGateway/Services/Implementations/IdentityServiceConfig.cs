using CommunityBox.Api.WebGateway.Services.Abstractions;

namespace CommunityBox.Api.WebGateway.Services.Implementations
{
    public class IdentityServiceConfig : IExternalServiceConfig
    {
        public string ApplicationUrl { get; set; }
    }
}