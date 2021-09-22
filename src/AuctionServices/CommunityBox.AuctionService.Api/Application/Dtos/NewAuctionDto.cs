using System;

namespace CommunityBox.AuctionService.Api.Application.Dtos
{
    public class NewAuctionDto
    {
        public string OwnerUserId { get; set; }
        
        public NewLotDto Lot { get; set; }
        
        public TimeSpan Duration { get; set; }
        
        public decimal StartingPrice { get; set; }
    }
}