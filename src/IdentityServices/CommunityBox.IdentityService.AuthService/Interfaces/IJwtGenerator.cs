namespace CommunityBox.IdentityService.AuthService.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(string email, string userId);

        string RefreshToken(string token, string email, string userId);
    }
}