using CommunityBox.IdentityService.Api;
using CommunityBox.IdentityService.Api.Databases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CommunityBox.IdentityService.IntegrationTests.Infrastructure
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            
            services.Replace(ServiceDescriptor.Singleton<IdentityContext, IdentityContextFixture>());
        }
    }
}