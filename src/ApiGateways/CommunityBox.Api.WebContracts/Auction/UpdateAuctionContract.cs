using System;

namespace CommunityBox.Api.WebContracts.Auction
{
    public class UpdateAuctionContract
    {
        public LotContract Lot { get; set; }

        public TimeSpan Duration { get; set; }
    }
}