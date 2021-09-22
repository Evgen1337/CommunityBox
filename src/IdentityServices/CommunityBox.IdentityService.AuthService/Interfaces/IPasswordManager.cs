using CommunityBox.IdentityService.AuthService.Models;

namespace CommunityBox.IdentityService.AuthService.Interfaces
{
    public interface IPasswordManager
    {
        string GetNewPassword();

        PasswordValidateResponse ValidatePassword(string password);
    }
}