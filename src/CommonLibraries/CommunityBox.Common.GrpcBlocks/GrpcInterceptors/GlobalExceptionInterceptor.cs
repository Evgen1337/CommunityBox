using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace CommunityBox.Common.GrpcBlocks.GrpcInterceptors
{
    public class GlobalExceptionInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (Exception e)
            {
                var status = new Status(StatusCode.Cancelled, e.Message + " - " + e.StackTrace);
                throw new RpcException(status);
            }
        }
    }
}