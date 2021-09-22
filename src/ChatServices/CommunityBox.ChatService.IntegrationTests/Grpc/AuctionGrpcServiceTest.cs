using System.Threading.Tasks;
using CommunityBox.ChatService.Api.Proto;
using CommunityBox.ChatService.IntegrationTests.Infrastructure;
using Xunit;

namespace CommunityBox.ChatService.IntegrationTests.Grpc
{
    public class ChatGrpcServiceTest : TestApiBase<TestStartup>
    {
        private const string ApplicationUrl = "http://localhost:5005";

        /*[Fact]
        public async Task JoinAtChat_OK()
        {
            using var channel = PrepareTestAndGetGrpcChannel(ApplicationUrl);
            var client = new ChatServices.ChatServicesClient(channel);

            using var call = client.JoinAtMessenger();
            
            using (var chat = client.JoinAtMessenger())
            {
                var task = Task.Run(async () =>
                {
                    while (await call.ResponseStream.MoveNext(CancellationToken.None))
                    {
                    }
                });

                await call.RequestStream.WriteAsync(new JoinAtMessengerRequest
                {
                    ChatId = 1,
                    RequestType = JoinAtMessengerRequestType.GetMessages
                });
            }
        }*/


        [Fact]
        public async Task GetChats_OK()
        {
            using var channel = PrepareTestAndGetGrpcChannel(ApplicationUrl);
            var client = new ChatServices.ChatServicesClient(channel);

            var retval = await client.GetChatsAsync(new GetChatsRequest
            {
                UserId = "8f5a2e6c-b14b-4691-a0d7-4c6ebab1d58f"
            });
        }
    }
}