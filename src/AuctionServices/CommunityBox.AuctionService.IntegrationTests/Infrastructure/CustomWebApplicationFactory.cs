using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityBox.AuctionService.IntegrationTests.Infrastructure
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        private readonly Action<IConfiguration, IServiceCollection> _action;

        public CustomWebApplicationFactory(Action<IConfiguration, IServiceCollection> action)
        {
            _action = action;
        }

        private IConfiguration Configuration { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseContentRoot(TestBase<TStartup>.SolutionRelativeContentRoot)
                .ConfigureServices(collection => _action?.Invoke(Configuration, collection));
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, false)
                .Build();

            return new WebHostBuilder()
                .UseConfiguration(Configuration)
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<TStartup>();
        }
    }
}