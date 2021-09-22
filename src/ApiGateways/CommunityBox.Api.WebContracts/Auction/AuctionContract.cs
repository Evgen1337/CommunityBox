using System;
using System.Collections.Generic;

namespace CommunityBox.Api.WebContracts.Auction
{
    public class AuctionContract
    {
        public long Id { get; set; }

        public string OwnerUserId { get; set; }

        public LotContract Lot { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime CreationUtcDate { get; set; }

        public DateTime? UpdateUtcDate { get; set; }

        public decimal StartingPrice { get; set; }

        public IReadOnlyCollection<AuctioneerContract> Auctioneers { get; set; }

        public IReadOnlyCollection<SubscriberContract> Subscribers { get; set; }
    }
}