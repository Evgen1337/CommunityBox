using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityBox.ChatService.Domain.Abstractions;
using CommunityBox.ChatService.Domain.Entities;
using CommunityBox.Common.Core;
using CommunityBox.Common.Core.SystemChat;
using CommunityBox.Common.Kafka.Consumer.Abstractions;
using CommunityBox.Common.Kafka.Messages;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;

namespace CommunityBox.ChatService.Api.Services.Kafka
{
    public class SubscriberMessageHandler : IKafkaHandler<Null, SubscriberMessageContent>
    {
        private readonly IChatRepository _chatRepository;

        public SubscriberMessageHandler(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task HandleAsync(Message<Null, SubscriberMessageContent> message)
        {
            var chatMessageContent = GetMessageContent(message.Value);

            await _chatRepository.CreateOrAddOnChatMessageAsync(
                SystemChatSettings.SystemUserId,
                message.Value.AuctionOwnerUserId,
                chatMessageContent
            );
        }

        private static string GetMessageContent(SubscriberMessageContent messageValue)
        {
            return messageValue.Action switch
            {
                SubscriberAction.Sub => SystemChatMessageTemplates.GetOnSubMessageTemplate(messageValue.UserId,
                    messageValue.AuctionId),

                SubscriberAction.Unsub => SystemChatMessageTemplates.GetOnUnsubMessageTemplate(messageValue.UserId,
                    messageValue.AuctionId),

                SubscriberAction.None => throw new ArgumentOutOfRangeException(nameof(messageValue.Action)),
                _ => throw new ArgumentOutOfRangeException(nameof(messageValue.Action))
            };
        }
    }
}