using AutoMapper;
using CommunityBox.Api.WebContracts.Auction;
using CommunityBox.Api.WebGateway.Protos;
using CommunityBox.AuctionService.Api.Proto;
using CommunityBox.Common.GrpcBlocks.GrpcTypes;
using Google.Protobuf.WellKnownTypes;

namespace CommunityBox.Api.WebGateway.Mapper.Profiles
{
    public class AuctionServiceMapperProfile : Profile
    {
        public AuctionServiceMapperProfile()
        {
            CreateMap<NewAuctionModel, NewAuctionContract>()
                .ForMember(d => d.Duration, f => f.MapFrom(m => m.Duration.ToTimeSpan()))
                .ForMember(d => d.StartingPrice, f => f.MapFrom(m => m.StartingPrice.ToDecimal()));
            CreateMap<NewAuctionContract, NewAuctionModel>()
                .ForMember(d => d.Duration, f => f.MapFrom(m => Duration.FromTimeSpan(m.Duration)))
                .ForMember(d => d.StartingPrice, f => f.MapFrom(m => m.StartingPrice.ToDecimalValue()));

            CreateMap<UpdateAuctionModel, UpdateAuctionContract>()
                .ForMember(d => d.Duration, f => f.MapFrom(m => m.Duration.ToTimeSpan()));
            CreateMap<UpdateAuctionContract, UpdateAuctionModel>()
                .ForMember(d => d.Duration, f => f.MapFrom(m => Duration.FromTimeSpan(m.Duration)));

            CreateMap<AuctionModel, AuctionContract>()
                .ForMember(d => d.CreationUtcDate, f => f.MapFrom(m => m.CreationUtcDate.ToDateTime()))
                .ForMember(d => d.UpdateUtcDate, f => f.MapFrom(m => m.UpdateUtcDate.ToDateTime()))
                .ForMember(d => d.Duration, f => f.MapFrom(m => m.Duration.ToTimeSpan()))
                .ForMember(d => d.StartingPrice, f => f.MapFrom(m => m.StartingPrice.ToDecimal()));

            CreateMap<AuctionContract, AuctionModel>()
                .ForMember(d => d.CreationUtcDate,
                    f => f.MapFrom(m => m.CreationUtcDate.ToTimestampWithKindUtc()))
                .ForMember(d => d.UpdateUtcDate,
                    f => f.MapFrom(m =>
                        m.UpdateUtcDate.ToTimestampWithKindUtc()))
                .ForMember(d => d.Duration, f => f.MapFrom(m => Duration.FromTimeSpan(m.Duration)))
                .ForMember(d => d.StartingPrice, f => f.MapFrom(m => m.StartingPrice.ToDecimalValue()));

            CreateMap<LotContract, LotModel>();
            CreateMap<LotModel, LotContract>();

            CreateMap<NewLotContract, NewLotModel>();

            CreateMap<AuctioneerContract, AuctioneerModel>()
                .ForMember(d => d.Bet, f => f.MapFrom(m => m.Bet.ToDecimalValue()));
            CreateMap<AuctioneerModel, AuctioneerContract>()
                .ForMember(d => d.Bet, f => f.MapFrom(m => m.Bet.ToDecimal()));
            
            CreateMap<SubscriberContract, SubscriberModel>();
            CreateMap<SubscriberModel, SubscriberContract>();
        }
    }
}