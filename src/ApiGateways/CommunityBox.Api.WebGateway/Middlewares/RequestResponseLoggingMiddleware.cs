/*using System.IO;
using System.Threading.Tasks;
using FriendlyCards.Backend.Api.LogsServices;
using Microsoft.AspNetCore.Http;
using NLog;

namespace FriendlyCards.Backend.Api.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILoggerProvider loggerProvider,
            ILogRequestResponseService logRequestResponseService)
        {
            var logger = loggerProvider.GetLogger<RequestResponseLoggingMiddleware>();

            var request = context.Request;
            request.EnableBuffering();
            
            var requestRetval = await logRequestResponseService.GetRequestAsync(request.Body, request);
            logger.LogRequest(requestRetval);

            var originalBodyStream = context.Response.Body;

            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            var responseRetval = await logRequestResponseService.GetResponseAsync(context.Response);
            logger.LogResponse(responseRetval);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}*/