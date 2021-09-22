namespace CommunityBox.Api.WebGateway.Services.Abstractions
{
    public interface IExternalService<out T> where T : IExternalServiceConfig
    {
        public IExternalServiceConfig ServiceConfig { get; }
    }
}