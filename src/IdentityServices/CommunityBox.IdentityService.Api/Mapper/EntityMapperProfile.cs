using System;
using AutoMapper;
using CommunityBox.IdentityService.Api.Domain.Entities;
using CommunityBox.IdentityService.Api.Proto;
using Google.Protobuf.WellKnownTypes;

namespace CommunityBox.IdentityService.Api.Mapper
{
    public class EntityMapperProfile : Profile
    {
        public EntityMapperProfile()
        {
            CreateMap<NewUserModel, User>();

            CreateMap<UpdateUserModel, User>();

            CreateMap<UserModel, User>()
                .ForMember(d => d.CreationUtcDate, f => f.MapFrom(m => m.CreationUtcDate.ToDateTime()))
                .ForMember(d => d.UpdateUtcDate, f => f.MapFrom(m => m.UpdateUtcDate.ToDateTime()));
            CreateMap<User, UserModel>()
                .ForMember(d => d.CreationUtcDate, f => f.MapFrom(m => ToTimestamp(m.CreationUtcDate)))
                .ForMember(d => d.UpdateUtcDate, f => f.MapFrom(m => ToTimestamp(m.UpdateUtcDate)));

            CreateMap<UserPersonalInformationModel, UserPersonalInformation>()
                .ForMember(d => d.BirthDay, f => f.MapFrom(m => m.BirthDay.ToDateTime()));
            CreateMap<UserPersonalInformation, UserPersonalInformationModel>()
                .ForMember(d => d.BirthDay, f => f.MapFrom(m => ToTimestamp(m.BirthDay)));

            CreateMap<AccountSettingModel, AccountSetting>();
            CreateMap<AccountSetting, AccountSettingModel>();
        }

        private static Timestamp ToTimestamp(DateTime? dateTime) =>
            dateTime.HasValue
                ? DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc).ToTimestamp()
                : null;
    }
}