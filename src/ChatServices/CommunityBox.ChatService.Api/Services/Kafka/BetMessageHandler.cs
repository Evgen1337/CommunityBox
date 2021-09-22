using System;
using System.Threading.Tasks;
using CommunityBox.Common.Core;
using CommunityBox.Common.Core.SystemChat;
using CommunityBox.Common.Kafka.Consumer.Abstractions;
using CommunityBox.Common.Kafka.Messages;
using Confluent.Kafka;

namespace CommunityBox.ChatService.Api.Services.Kafka
{
    public class BetMessageHandler : IKafkaHandler<Null, BetMessageContent>
    {
        private readonly IChatRepository _chatRepository;

        public BetMessageHandler(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task HandleAsync(Message<Null, BetMessageContent> message)
        {
            var chatMessageContent = GetMessageContent(message.Value);

            await _chatRepository.CreateOrAddOnChatMessageAsync(
                SystemChatSettings.SystemUserId,
                message.Value.AuctionOwnerUserId,
                chatMessageContent
            );
        }
        
        private static string GetMessageContent(BetMessageContent messageValue)
        {
            return messageValue.Action switch
            {
                BetAction.Set => SystemChatMessageTemplates.GetSetBetMessageTemplate(messageValue.UserId,
                    messageValue.AuctionId, messageValue.Value),

                BetAction.Remove => SystemChatMessageTemplates.GetRemoveBetMessageTemplate(messageValue.UserId,
                    messageValue.AuctionId),

                BetAction.None => throw new ArgumentOutOfRangeException(nameof(messageValue.Action)),
                _ => throw new ArgumentOutOfRangeException(nameof(messageValue.Action))
            };
        }
    }
}