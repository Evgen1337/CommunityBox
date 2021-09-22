using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityBox.AuctionService.IntegrationTests.Infrastructure
{
    public abstract class TestBase<TStartup>
        where TStartup : class
    {
        public static readonly string SolutionRelativeContentRoot =
            Path.GetDirectoryName(typeof(TStartup).Assembly.Location);

        protected CustomWebApplicationFactory<TStartup> Factory { get; private set; }

        protected CustomWebApplicationFactory<TStartup> CreateFactory(
            Action<IConfiguration, IServiceCollection> action = null)
        {
            Factory = new CustomWebApplicationFactory<TStartup>(action);
            return Factory;
        }
    }
}