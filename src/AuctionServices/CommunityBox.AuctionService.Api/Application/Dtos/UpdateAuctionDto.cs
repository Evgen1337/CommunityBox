using System;

namespace CommunityBox.AuctionService.Api.Application.Dtos
{
    public class UpdateAuctionDto
    {
        public long? Id { get; set; }

        public string OwnerUserId { get; set; } 
        
        public LotDto Lot { get; set; }

        public TimeSpan Duration { get; set; }
    }
}