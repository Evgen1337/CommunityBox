using System;
using System.Threading.Tasks;
using CommunityBox.Common.Logging.Abstractions;
using CommunityBox.Common.Logging.Implementations;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace CommunityBox.Common.GrpcBlocks.Interceptors
{
    public class GlobalExceptionInterceptor : Interceptor
    {
        private readonly ILoggerProvider _loggerProvider;

        public GlobalExceptionInterceptor(ILoggerProvider loggerProvider)
        {
            _loggerProvider = loggerProvider;
        }
        
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (Exception ex)
            {
                var logger = _loggerProvider.GetLogger<GlobalExceptionInterceptor>();
                logger.LogUnhandledErrorObj(ex, _loggerProvider.LoggerContext);
                
                var status = new Status(StatusCode.Cancelled, ex.Message);
                throw new RpcException(status);
            }
        }
    }
}