using AutoMapper;
using CommunityBox.AuctionService.Api.Application.Dtos;
using CommunityBox.AuctionService.Domain.Entities;

namespace CommunityBox.AuctionService.Api.Mapper.Profiles
{
    public class EntityMapperProfile : Profile
    {
        public EntityMapperProfile()
        {
            RegisterEntities();
        }

        private void RegisterEntities()
        {
            CreateMap<Auction, AuctionDto>();
            CreateMap<AuctionDto, Auction>();
            
            CreateMap<Auction, NewAuctionDto>();
            CreateMap<NewAuctionDto, Auction>();
            
            CreateMap<Auction, UpdateAuctionDto>();
            CreateMap<UpdateAuctionDto, Auction>();
            
            CreateMap<Auctioneer, AuctioneerDto>();
            CreateMap<AuctioneerDto, Auctioneer>();
            
            CreateMap<NewLotDto, Lot>();
            CreateMap<Lot, NewLotDto>();
            
            CreateMap<Lot, LotDto>();
            CreateMap<LotDto, Lot>();
            
            CreateMap<Subscriber, SubscriberDto>();
            CreateMap<SubscriberDto, Subscriber>();
        }
    }
}