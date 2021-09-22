using Microsoft.AspNetCore.Http;

namespace CommunityBox.IdentityService.AuthService.Interfaces
{
    public interface IJwtTokenManager
    {
        string GenerateToken(string email, string userId);

        string RefreshToken(string token, string email, string userId);

        string GetUserIdFromHeaderToken(HttpRequest request);

        bool ValidateToken(string token);
    }
}