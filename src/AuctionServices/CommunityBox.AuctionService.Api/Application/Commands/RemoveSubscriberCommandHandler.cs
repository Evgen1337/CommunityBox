using System.Threading;
using System.Threading.Tasks;
using CommunityBox.AuctionService.Domain.Abstractions;
using CommunityBox.AuctionService.Domain.Entities;
using CommunityBox.Common.Exceptions;
using CommunityBox.Common.Kafka.Messages;
using CommunityBox.Common.Kafka.Producer.Abstractions;
using Confluent.Kafka;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommunityBox.AuctionService.Api.Application.Commands
{
    public class RemoveSubscriberCommandHandler : IRequestHandler<RemoveSubscriberCommand>
    {
        private readonly IAuctionContext _auctionContext;
        private readonly IKafkaProducerMessageBus<Null, SubscriberMessageContent> _subscriberBus;

        public RemoveSubscriberCommandHandler(IAuctionContext auctionContext,
            IKafkaProducerMessageBus<Null, SubscriberMessageContent> subscriberBus)
        {
            _auctionContext = auctionContext;
            _subscriberBus = subscriberBus;
        }

        public async Task<Unit> Handle(RemoveSubscriberCommand request, CancellationToken cancellationToken)
        {
            var subscriber = await _auctionContext.QueryEntity<Subscriber>()
                .Include(i => i.Auction)
                .FirstOrDefaultAsync(f => f.AuctionId == request.AuctionId && f.UserId == request.UserId,
                    cancellationToken);

            if (subscriber == null)
                throw new EntityNotFoundException();

            _auctionContext.RemoveEntity(subscriber);
            await _auctionContext.SaveChangesAsync();

            await SendToKafkaAsync(request, subscriber.Auction.OwnerUserId);

            return Unit.Value;
        }

        private async Task SendToKafkaAsync(RemoveSubscriberCommand request, string auctionOwnerUserId)
        {
            var message = new Message<Null, SubscriberMessageContent>
            {
                Value = new SubscriberMessageContent
                {
                    Action = SubscriberAction.Unsub,
                    AuctionId = request.AuctionId,
                    UserId = request.UserId,
                    AuctionOwnerUserId = auctionOwnerUserId
                }
            };

            await _subscriberBus.PublishAsync(message);
        }
    }
}