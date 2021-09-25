using System.IO;
using System.Threading.Tasks;
using CommunityBox.Common.Logging.Abstractions;
using CommunityBox.Common.Logging.Implementations;
using Microsoft.AspNetCore.Http;

namespace CommunityBox.Common.Logging.Middlewares
{
    public sealed class HttpRequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpRequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILoggerProvider loggerProvider,
            ILogRequestResponseService logRequestResponseService)
        {
            var logger = loggerProvider.GetLogger<HttpRequestResponseLoggingMiddleware>();

            var request = context.Request;
            request.EnableBuffering();
            
            var requestRetval = await logRequestResponseService.GetHttpRequestAsync(request.Body, request);
            logger.LogRequest(requestRetval);

            var originalBodyStream = context.Response.Body;

            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            var responseRetval = await logRequestResponseService.GetHttpResponseAsync(context.Response);
            logger.LogResponse(responseRetval);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}