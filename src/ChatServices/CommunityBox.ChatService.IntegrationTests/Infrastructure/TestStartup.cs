using CommunityBox.ChatService.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityBox.ChatService.IntegrationTests.Infrastructure
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            
            //services.Replace(ServiceDescriptor.Singleton<IChatContext, ChatContextFixture>());
        }
    }
}