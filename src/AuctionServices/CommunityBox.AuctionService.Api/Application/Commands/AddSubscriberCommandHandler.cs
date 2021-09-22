using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityBox.AuctionService.Domain.Abstractions;
using CommunityBox.AuctionService.Domain.Entities;
using CommunityBox.Common.Kafka.Messages;
using CommunityBox.Common.Kafka.Producer.Abstractions;
using Confluent.Kafka;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace CommunityBox.AuctionService.Api.Application.Commands
{
    public class AddSubscriberCommandHandler : IRequestHandler<AddSubscriberCommand>
    {
        private readonly IAuctionContext _auctionContext;
        private readonly IKafkaProducerMessageBus<Null, SubscriberMessageContent> _subscriberBus;

        public AddSubscriberCommandHandler(IAuctionContext auctionContext,
            IKafkaProducerMessageBus<Null, SubscriberMessageContent> subscriberBus)
        {
            _auctionContext = auctionContext;
            _subscriberBus = subscriberBus;
        }

        public async Task<Unit> Handle(AddSubscriberCommand request, CancellationToken cancellationToken)
        {
            var auction = await _auctionContext.QueryEntity<Auction>()
                .Include(i => i.Subscribers)
                .FirstOrDefaultAsync(f => f.Id == request.AuctionId, cancellationToken);

            if (auction.Subscribers.Any(a => a.UserId == request.UserId))
                return Unit.Value;

            auction.Subscribers.Add(new Subscriber
            {
                Auction = auction,
                UserId = request.UserId
            });

            _auctionContext.UpdateEntity(auction);
            await _auctionContext.SaveChangesAsync();

            await SendToKafkaAsync(request, auction.OwnerUserId);

            return Unit.Value;
        }

        private async Task SendToKafkaAsync(AddSubscriberCommand request, string auctionOwnerUserId)
        {
            var message = new Message<Null, SubscriberMessageContent>
            {
                Value = new SubscriberMessageContent
                {
                    Action = SubscriberAction.Sub,
                    AuctionId = request.AuctionId,
                    UserId = request.UserId,
                    AuctionOwnerUserId = auctionOwnerUserId
                }
            };

            await _subscriberBus.PublishAsync(message);
        }
    }
}