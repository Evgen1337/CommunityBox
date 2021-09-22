using CommunityBox.Api.WebGateway.Services.Abstractions;

namespace CommunityBox.Api.WebGateway.Services.Implementations
{
    public class AuctionServiceConfig : IExternalServiceConfig
    {
        public string ApplicationUrl { get; set; }
    }
}