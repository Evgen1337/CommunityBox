using System.IO;
using System.Threading.Tasks;
using CommunityBox.Common.Logging.Models;
using Microsoft.AspNetCore.Http;

namespace CommunityBox.Common.Logging.Abstractions
{
    public interface ILogRequestResponseService
    {
        public Task<LoggerRequestData> GetHttpRequestAsync(Stream content, HttpRequest request);
        
        public LoggerRequestData GetGrpcRequest<T>(T content, HttpRequest request);

        public Task<LoggerResponseData> GetHttpResponseAsync(HttpResponse response);
        
        public LoggerResponseData GetGrpcResponse<T>(T content, HttpResponse response);
    }
}