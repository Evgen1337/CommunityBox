using System.IO;
using System.Threading.Tasks;
using CommunityBox.Common.Logging.Abstractions;
using CommunityBox.Common.Logging.Implementations;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace CommunityBox.Common.Logging.Interceptors
{
    public class RequestResponseLoggingInterceptor : Interceptor
    {
        private readonly ILoggerProvider _loggerProvider;
        private readonly ILogRequestResponseService _logRequestResponseService;

        public RequestResponseLoggingInterceptor(ILoggerProvider loggerProvider,
            ILogRequestResponseService logRequestResponseService)
        {
            _loggerProvider = loggerProvider;
            _logRequestResponseService = logRequestResponseService;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var logger = _loggerProvider.GetLogger<RequestResponseLoggingInterceptor>();

            var httpContext = context.GetHttpContext();
            var httpRequest = httpContext.Request;
            
            var requestRetval = _logRequestResponseService.GetGrpcRequest(request, httpRequest);
            logger.LogRequest(requestRetval);

            var response = await continuation(request, context);
            
            var responseRetval = _logRequestResponseService.GetGrpcResponse(response, httpContext.Response);
            logger.LogResponse(responseRetval);

            return response;
        }
    }
}