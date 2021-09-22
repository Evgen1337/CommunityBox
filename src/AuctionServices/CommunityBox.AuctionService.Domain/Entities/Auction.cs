using System;
using System.Collections.Generic;
using CommunityBox.Common.Core;

namespace CommunityBox.AuctionService.Domain.Entities
{
    public class Auction : IEntity
    {
        public long Id { get; set; }

        public string OwnerUserId { get; set; }

        public Lot Lot { get; set; }

        public TimeSpan? Duration { get; set; }
        
        public DateTime CreationUtcDate { get; set; }

        public DateTime? UpdateUtcDate { get; set; }

        public decimal StartingPrice { get; set; }

        public ICollection<Subscriber> Subscribers { get; set; }

        public ICollection<Auctioneer> Auctioneers { get; set; }
    }
}