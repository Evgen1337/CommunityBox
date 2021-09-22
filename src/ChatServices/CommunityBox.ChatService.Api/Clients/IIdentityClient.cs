using System.Threading.Tasks;
using CommunityBox.IdentityService.Api.Proto;

namespace CommunityBox.ChatService.Api.Clients
{
    public interface IIdentityClient
    {
        Task<UserModel> GetUserAsync (string userId);
    }
}