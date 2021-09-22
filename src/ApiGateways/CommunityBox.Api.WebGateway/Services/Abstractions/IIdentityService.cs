using System.Threading.Tasks;
using CommunityBox.IdentityService.Api.Proto;
using Microsoft.AspNetCore.Http;

namespace CommunityBox.Api.WebGateway.Services.Abstractions
{
    public interface IIdentityService
    {
        Task<AuthModel> AuthAsync(LogInModel requestModel);

        Task<bool> ValidateTokenAsync(HttpRequest httpRequest);

        Task<UserModel> CreateUserAsync(NewUserModel requestModel);

        Task<UserModel> GetUserAsync(string id);

        Task<UserModel> UpdateUserAsync(UpdateUserModel requestModel, HttpRequest httpRequest);
    }
}