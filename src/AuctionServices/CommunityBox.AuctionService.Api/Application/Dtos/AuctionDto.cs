using System;
using System.Collections.Generic;

namespace CommunityBox.AuctionService.Api.Application.Dtos
{
    public class AuctionDto
    {
        public long? Id { get; set; }

        public string OwnerUserId { get; set; }
        
        public LotDto Lot { get; set; }

        public TimeSpan Duration { get; set; }
        
        public DateTime CreationUtcDate { get; set; }

        public DateTime? UpdateUtcDate { get; set; }

        public decimal StartingPrice { get; set; }

        public IReadOnlyCollection<SubscriberDto> Subscribers { get; set; }

        public IReadOnlyCollection<AuctioneerDto> Auctioneers { get; set; }
    }
}