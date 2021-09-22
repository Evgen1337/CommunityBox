using System;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace CommunityBox.Api.WebGateway.Services.Abstractions
{
    public abstract class BaseExternalService : IExternalService<IExternalServiceConfig>
    {
        public IExternalServiceConfig ServiceConfig { get; }

        protected BaseExternalService(IExternalServiceConfig serviceConfig)
        {
            ServiceConfig = serviceConfig;
        }

        protected async Task<TResponse> CallServiceAsync<TResponse>(Func<GrpcChannel, Task<TResponse>> func)
        {
            using var channel = GrpcChannel.ForAddress(ServiceConfig.ApplicationUrl);
            return await func(channel);
        }
    }
}