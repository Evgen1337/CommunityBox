using System.Linq;
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
    public class SetBetCommandHandler : IRequestHandler<SetBetCommand>
    {
        private readonly IAuctionContext _auctionContext;
        private readonly IKafkaProducerMessageBus<Null, BetMessageContent> _betBus;

        public SetBetCommandHandler(IKafkaProducerMessageBus<Null, BetMessageContent> betBus,
            IAuctionContext auctionContext)
        {
            _betBus = betBus;
            _auctionContext = auctionContext;
        }

        public async Task<Unit> Handle(SetBetCommand request, CancellationToken cancellationToken)
        {
            var auction = await _auctionContext.QueryEntity<Auction>()
                .Include(i => i.Auctioneers)
                .FirstOrDefaultAsync(f => f.Id == request.AuctionId, cancellationToken);

            if (auction == null)
                throw new EntityNotFoundException();
            
            var auctioneer = auction.Auctioneers
                .FirstOrDefault(a => a.AuctionId == request.AuctionId && a.UserId == request.UserId);

            if (auctioneer == null)
            {
                auction.Auctioneers.Add(new Auctioneer
                {
                    Bet = request.Value,
                    Auction = auction,
                    UserId = request.UserId
                });
            }
            else
            {
                auctioneer.Bet = request.Value;
            }

            _auctionContext.UpdateEntity(auction);
            await _auctionContext.SaveChangesAsync();

            await SendToKafkaAsync(request, auction.OwnerUserId);
            return Unit.Value;
        }

        private async Task SendToKafkaAsync(SetBetCommand request, string auctionOwnerUserId)
        {
            var message = new Message<Null, BetMessageContent>
            {
                Value = new BetMessageContent
                {
                    Action = BetAction.Set,
                    AuctionId = request. AuctionId,
                    UserId = request.UserId,
                    Value = request.Value,
                    AuctionOwnerUserId = auctionOwnerUserId
                }
            };

            await _betBus.PublishAsync(message);
        }
    }
}