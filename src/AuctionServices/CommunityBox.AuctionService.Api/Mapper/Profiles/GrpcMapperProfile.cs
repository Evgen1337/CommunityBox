using AutoMapper;
using CommunityBox.AuctionService.Api.Application.Commands;
using CommunityBox.AuctionService.Api.Application.Dtos;
using CommunityBox.AuctionService.Api.Grpc;
using CommunityBox.AuctionService.Api.Proto;
using CommunityBox.Common.GrpcBlocks.GrpcTypes;
using Google.Protobuf.WellKnownTypes;

namespace CommunityBox.AuctionService.Api.Mapper.Profiles
{
    public class GrpcMapperProfile : Profile
    {
        public GrpcMapperProfile()
        {
            RegisterRequests();
            RegisterModels();
        }

        private void RegisterRequests()
        {
            CreateMap<AddSubscriberRequest, AddSubscriberCommand>();
            CreateMap<CreateAuctionRequest, CreateAuctionCommand>();
            CreateMap<UpdateAuctionRequest, UpdateAuctionCommand>();
            CreateMap<DeleteAuctionRequest, DeleteAuctionCommand>();
            CreateMap<RemoveBetRequest, RemoveBetCommand>();
            CreateMap<RemoveSubscriberRequest, RemoveSubscriberCommand>();
            CreateMap<SetBetRequest, SetBetCommand>()
                .ForMember(d => d.Value, f => f.MapFrom(m => m.Value.ToDecimal()));
        }

        private void RegisterModels()
        {
            CreateMap<NewAuctionModel, NewAuctionDto>()
                .ForMember(d => d.Duration, f => f.MapFrom(m => m.Duration.ToTimeSpan()))
                .ForMember(d => d.StartingPrice, f => f.MapFrom(m => m.StartingPrice.ToDecimal()));
            CreateMap<NewAuctionDto, NewAuctionModel>()
                .ForMember(d => d.Duration, f => f.MapFrom(m => Duration.FromTimeSpan(m.Duration)))
                .ForMember(d => d.StartingPrice, f => f.MapFrom(m => m.StartingPrice.ToDecimalValue()));

            CreateMap<UpdateAuctionModel, UpdateAuctionDto>()
                .ForMember(d => d.Duration, f => f.MapFrom(m => m.Duration.ToTimeSpan()));
            CreateMap<UpdateAuctionDto, UpdateAuctionModel>()
                .ForMember(d => d.Duration, f => f.MapFrom(m => Duration.FromTimeSpan(m.Duration)));

            CreateMap<AuctionModel, AuctionDto>()
                .ForMember(d => d.CreationUtcDate, f => f.MapFrom(m => m.CreationUtcDate.ToDateTime()))
                .ForMember(d => d.UpdateUtcDate, f => f.MapFrom(m => m.UpdateUtcDate.ToDateTime()))
                .ForMember(d => d.Duration, f => f.MapFrom(m => m.Duration.ToTimeSpan()))
                .ForMember(d => d.StartingPrice, f => f.MapFrom(m => m.StartingPrice.ToDecimal()));

            CreateMap<AuctionDto, AuctionModel>()
                .ForMember(d => d.CreationUtcDate,
                    f => f.MapFrom(m => m.CreationUtcDate.ToTimestampWithKindUtc()))
                .ForMember(d => d.UpdateUtcDate,
                    f => f.MapFrom(m => m.UpdateUtcDate.ToTimestampWithKindUtc()))
                .ForMember(d => d.Duration, f => f.MapFrom(m => Duration.FromTimeSpan(m.Duration)))
                .ForMember(d => d.StartingPrice, f => f.MapFrom(m => m.StartingPrice.ToDecimalValue()));


            CreateMap<LotModel, LotDto>();
            CreateMap<LotDto, LotModel>();

            CreateMap<NewLotModel, NewLotDto>();
            CreateMap<NewLotDto, NewLotModel>();

            CreateMap<AuctioneerModel, AuctioneerDto>()
                .ForMember(d => d.Bet, f => f.MapFrom(m => m.Bet.ToDecimal()));
            CreateMap<AuctioneerDto, AuctioneerModel>()
                .ForMember(d => d.Bet, f => f.MapFrom(m => m.Bet.ToDecimalValue()));
            
            CreateMap<SubscriberModel, SubscriberDto>();
            CreateMap<SubscriberDto, SubscriberModel>();
        }
    }
}