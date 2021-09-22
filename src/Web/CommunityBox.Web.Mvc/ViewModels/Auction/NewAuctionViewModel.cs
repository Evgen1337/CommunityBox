using System;

namespace CommunityBox.Web.Mvc.ViewModels.Auction
{
    public class NewAuctionViewModel
    {
        public NewLotViewModel Lot { get; set; }
        
        public TimeSpan Duration { get; set; }
        
        public decimal StartingPrice { get; set; }
    }
}