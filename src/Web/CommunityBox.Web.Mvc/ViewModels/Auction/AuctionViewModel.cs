using System;
using System.Collections.Generic;

namespace CommunityBox.Web.Mvc.ViewModels.Auction
{
    public class AuctionViewModel
    {
        public long Id { get; set; }

        public string OwnerUserId { get; set; }

        public LotViewModel Lot { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime CreationUtcDate { get; set; }

        public DateTime? UpdateUtcDate { get; set; }

        public decimal StartingPrice { get; set; }

        public IReadOnlyCollection<AuctioneerViewModel> Auctioneers { get; set; }

        public IReadOnlyCollection<SubscriberViewModel> Subscribers { get; set; }

        public bool IsSubscribed { get; set; }
        
        public decimal? UserBet { get; set; }
    }
}