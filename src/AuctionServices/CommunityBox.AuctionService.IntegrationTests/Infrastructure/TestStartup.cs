using CommunityBox.AuctionService.Api;
using CommunityBox.AuctionService.Domain.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CommunityBox.AuctionService.IntegrationTests.Infrastructure
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            
            services.Replace(ServiceDescriptor.Singleton<IAuctionContext, AuctionContextFixture>());
        }
    }
}