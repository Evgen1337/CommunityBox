using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommunityBox.Web.Mvc.Configurations;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Http;

namespace CommunityBox.Web.Mvc.Clients.Gateway
{
    public abstract class BaseGatewayClient
    {
        protected readonly GatewayClientConfig ClientConfig;
        private readonly HttpContext _httpContext;
        private readonly IFlurlClient _flurlClient;
        private readonly NewtonsoftJsonSerializer _jsonSerializer;

        protected BaseGatewayClient(IFlurlClient flurlClient, NewtonsoftJsonSerializer jsonSerializer,
            GatewayClientConfig gatewayClientConfig, HttpContext httpContext)
        {
            _flurlClient = flurlClient;
            ClientConfig = gatewayClientConfig;
            _httpContext = httpContext;
            _jsonSerializer = jsonSerializer;
        }

        protected async Task<TResponse> SendRequestAsync<TResponse, TRequest>(TRequest requestContent, string route,
            HttpMethod method, bool withAuth = false)
        {
            var content = GenerateContent(requestContent);
            var request = GenerateRequest(route, withAuth);
            var response = await request.SendAsync(method, content);

            var retval = await ValidateResponseAndGetDataAsync<TResponse>(response);
            return retval;
        }
        
        protected async Task SendRequestAsync<TRequest>(TRequest requestContent, string route,
            HttpMethod method, bool withAuth = false)
        {
            var content = GenerateContent(requestContent);
            var request = GenerateRequest(route, withAuth);
            var response = await request.SendAsync(method, content);

            ValidateResponse(response);
        }

        protected async Task<TResponse> SendRequestAsync<TResponse>(string route, HttpMethod method,
            bool withAuth = false)
        {
            var request = GenerateRequest(route, withAuth);
            var response = await request.SendAsync(method);

            var retval = await ValidateResponseAndGetDataAsync<TResponse>(response);
            return retval;
        }

        protected async Task SendRequestAsync(string route, HttpMethod method, bool withAuth = false)
        {
            var request = GenerateRequest(route, withAuth);
            var response = await request.SendAsync(method);

            ValidateResponse(response);
        }

        private async Task<TResponse> ValidateResponseAndGetDataAsync<TResponse>(IFlurlResponse response)
        {
            ValidateResponse(response);

            var responseData = await response.GetJsonAsync<TResponse>();
            return responseData;
        }

        private IFlurlRequest GenerateRequest(string route, bool withAuth = false)
        {
            var url = CombineUrl(route);
            var request = _flurlClient.Request(url);

            if (withAuth)
                request.SetAuthHeaderFromContext(_httpContext);

            return request;
        }

        private void ValidateResponse(IFlurlResponse response)
        {
            if (response.StatusCode != 200 && response.StatusCode != 204)
            {
                throw new Exception();
            }
        }

        private StringContent GenerateContent<T>(T contentData)
        {
            var json = _jsonSerializer.Serialize(contentData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }

        private string CombineUrl(string methodRoute) =>
            ClientConfig.BaseUrl.AppendPathSegment(methodRoute);
    }
}