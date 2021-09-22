using AutoMapper;
using CommunityBox.ChatService.Api.Proto;
using CommunityBox.Web.Mvc.ViewModels.Chat;

namespace CommunityBox.Web.Mvc.Mapper
{
    public class ChatServiceMapperProfile : Profile
    {
        public ChatServiceMapperProfile()
        {
            CreateMap<NewSingleMessageViewModel, NewSingleMessageModel>();

            CreateMap<ChatPreviewModel, ChatPreviewViewModel>()
                .ForMember(f => f.LastMessageReceivedDate, d => d.MapFrom(a => a.LastMessageReceivedDate.ToDateTime()));
        }
    }
}