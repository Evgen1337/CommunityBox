using System.Net.Http;
using AutoFixture;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace CommunityBox.ChatService.IntegrationTests.Infrastructure
{
    public class TestApiBase<TStartup> : TestBase<TStartup>
        where TStartup : class
    {
        protected IConfiguration Configuration { get; set; }
        
        protected readonly Fixture Fixture = new();

        protected HttpClient PrepareTest()
        {
            CreateFactory();
            return Factory.CreateClient();
        }

        protected GrpcChannel PrepareTestAndGetGrpcChannel(string applicationUrl)
        {
            var httpClient = PrepareTest();
            
            var channel = GrpcChannel.ForAddress(applicationUrl, new GrpcChannelOptions
            {
                HttpClient = httpClient
            });

            return channel;
        }
    }
}