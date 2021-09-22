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
    public class RemoveBetCommandHandler : IRequestHandler<RemoveBetCommand>
    {
        private readonly IAuctionContext _auctionContext;
        private readonly IKafkaProducerMessageBus<Null, BetMessageContent> _betBus;

        public RemoveBetCommandHandler(IAuctionContext auctionContext,
            IKafkaProducerMessageBus<Null, BetMessageContent> betBus)
        {
            _auctionContext = auctionContext;
            _betBus = betBus;
        }

        public async Task<Unit> Handle(RemoveBetCommand request, CancellationToken cancellationToken)
        {
            var auctioneer = await _auctionContext.QueryEntity<Auctioneer>()
                .Include(i => i.Auction)
                .FirstOrDefaultAsync(f => f.AuctionId == request.AuctionId && f.UserId == request.UserId,
                    cancellationToken);

            if (auctioneer == null)
                throw new EntityNotFoundException();

            _auctionContext.RemoveEntity(auctioneer);
            await _auctionContext.SaveChangesAsync();

            await SendToKafkaAsync(request, auctioneer.Auction.OwnerUserId);

            return Unit.Value;
        }

        private async Task SendToKafkaAsync(RemoveBetCommand request, string auctionOwnerUserId)
        {
            var message = new Message<Null, BetMessageContent>
            {
                Value = new BetMessageContent
                {
                    Action = BetAction.Remove,
                    AuctionId = request.AuctionId,
                    UserId = request.UserId,
                    AuctionOwnerUserId = auctionOwnerUserId
                }
            };

            await _betBus.PublishAsync(message);
        }
    }
}