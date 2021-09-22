using AutoMapper;
using CommunityBox.Api.WebContracts.Auction;
using CommunityBox.Api.WebContracts.Identities;
using CommunityBox.Web.Mvc.ViewModels.Auction;
using CommunityBox.Web.Mvc.ViewModels.Identities;

namespace CommunityBox.Web.Mvc.Mapper
{
    public class GatewayMapperProfile : Profile
    {
        public GatewayMapperProfile()
        {
            RegisterAuction();
            RegisterIdentity();
        }
        
        private void RegisterAuction()
        {
            CreateMap<AuctioneerViewModel,  AuctioneerContract>();
            CreateMap<AuctioneerContract, AuctioneerViewModel>();
            
            CreateMap<AuctionViewModel, AuctionContract>();
            CreateMap<AuctionContract, AuctionViewModel>();
            
            CreateMap<LotViewModel, LotContract>();
            CreateMap<LotContract, LotViewModel>();
            
            CreateMap<NewAuctionViewModel, NewAuctionContract>();
            CreateMap<NewAuctionContract, NewAuctionViewModel>();
            
            CreateMap<NewLotViewModel, NewLotContract>();
            CreateMap<NewLotContract, NewLotViewModel>();
            
            CreateMap<SubscriberViewModel, SubscriberContract>();
            CreateMap<SubscriberContract, SubscriberViewModel>();
            
            CreateMap<UpdateAuctionViewModel, UpdateAuctionContract>();
            CreateMap<UpdateAuctionContract, UpdateAuctionViewModel>();
            CreateMap<AuctionContract, UpdateAuctionViewModel>();
            
            CreateMap<SubscribeViewModel, AddSubscriptionContract>();
        }
        
        private void RegisterIdentity()
        {
            CreateMap<AccountSettingViewModel, AccountSettingContract>();
            CreateMap<AccountSettingContract, AccountSettingViewModel>();
            
            CreateMap<AuthViewModel, AuthContract>();
            CreateMap<AuthContract, AuthViewModel>();
            
            CreateMap<LogInViewModel, LogInContract>();
            CreateMap<LogInContract, LogInViewModel>();
            
            CreateMap<NewUserViewModel, NewUserContract>();
            CreateMap<NewUserContract, NewUserViewModel>();
            
            CreateMap<UpdateUserViewModel, UpdateUserContract>();
            CreateMap<UpdateUserContract, UpdateUserViewModel>();
            
            CreateMap<UserPersonalInformationViewModel, UserPersonalInformationContract>();
            CreateMap<UserPersonalInformationContract, UserPersonalInformationViewModel>();
            
            CreateMap<UserViewModel, UserContract>();
            CreateMap<UserContract, UserViewModel>();
        }
    }
}