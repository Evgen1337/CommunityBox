using System;

namespace CommunityBox.Api.WebContracts.Auction
{
    public class NewAuctionContract
    {
        public NewLotContract Lot { get; set; }
        
        public TimeSpan Duration { get; set; }
        
        public decimal StartingPrice { get; set; }
    }
}