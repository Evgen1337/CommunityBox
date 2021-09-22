using System;

namespace CommunityBox.Web.Mvc.ViewModels.Auction
{
    public class UpdateAuctionViewModel
    {
        public LotViewModel Lot { get; set; }

        public TimeSpan Duration { get; set; }
    }
}