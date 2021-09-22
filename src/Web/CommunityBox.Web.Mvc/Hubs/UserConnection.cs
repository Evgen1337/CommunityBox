using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityBox.ChatService.Api.Proto;
using Grpc.Core;
using Grpc.Net.Client;

namespace CommunityBox.Web.Mvc.Hubs
{
    public class UserConnection : IDisposable
    {
        public string UserId { get; set; }

        public string ConnectionId { get; set; }

        public AsyncDuplexStreamingCall<JoinAtMessengerRequest, JoinAtMessengerResponse> GrpcStreaming { get; set; }

        public GrpcChannel GrpcChannel { get; set; }

        public Task GrpcStreamingTask { get; set; }

        public CancellationTokenSource CsTokenSource { get; set; }

        public void Dispose()
        {
            CsTokenSource?.Cancel();

            GrpcStreaming?.Dispose();
            GrpcChannel?.Dispose();
            GrpcStreamingTask?.Dispose();
            CsTokenSource?.Dispose();
        }
    }
}