using AutoMapper;
using CommunityBox.Api.WebContracts.Identities;
using CommunityBox.Common.GrpcBlocks.GrpcTypes;
using CommunityBox.IdentityService.Api.Proto;

namespace CommunityBox.Api.WebGateway.Mapper.Profiles
{
    public class IdentityServiceMapperProfile : Profile
    {
        public IdentityServiceMapperProfile()
        {
            CreateMap<NewUserContract, NewUserModel>();

            CreateMap<UpdateUserContract, UpdateUserModel>();

            CreateMap<UserContract, UserModel>()
                .ForMember(d => d.UpdateUtcDate,
                    f => f.MapFrom(m => m.UpdateUtcDate.ToTimestampWithKindUtc()))
                .ForMember(d => d.CreationUtcDate,
                    f => f.MapFrom(m => m.CreationUtcDate.ToTimestampWithKindUtc()));
            CreateMap<UserModel, UserContract>()
                .ForMember(d => d.UpdateUtcDate,
                    f => f.MapFrom(m => m.UpdateUtcDate.ToDateTime()))
                .ForMember(d => d.CreationUtcDate,
                    f => f.MapFrom(m => m.CreationUtcDate.ToDateTime()));

            CreateMap<UserPersonalInformationContract, UserPersonalInformationModel>()
                .ForMember(d => d.BirthDay,
                    f => f.MapFrom(m => m.BirthDay.ToTimestampWithKindUtc()));
            CreateMap<UserPersonalInformationModel, UserPersonalInformationContract>()
                .ForMember(d => d.BirthDay,
                    f => f.MapFrom(m => m.BirthDay.ToDateTime()));

            CreateMap<AuthContract, AuthModel>();
            CreateMap<AuthModel, AuthContract>();

            CreateMap<AccountSettingContract, AccountSettingModel>();
            CreateMap<AccountSettingModel, AccountSettingContract>();

            CreateMap<LogInContract, LogInModel>();
            CreateMap<LogInModel, LogInContract>();
        }
    }
}