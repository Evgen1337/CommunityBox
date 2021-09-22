using System.Collections.Generic;
using System.Linq;
using CodeJam.Strings;
using CommunityBox.IdentityService.AuthService.Interfaces;
using CommunityBox.IdentityService.AuthService.Models;
using PasswordGenerator;

namespace CommunityBox.IdentityService.AuthService.Implementations
{
    public class PasswordManager : IPasswordManager
    {
        private readonly Password _password;
        private readonly IPasswordSettings _passwordSettings;

        public PasswordManager(IPasswordSettings passwordSettings)
        {
            _passwordSettings = passwordSettings;
            _password = new Password(passwordSettings);
        }

        public string GetNewPassword()
        {
            return _password.Next();
        }

        public PasswordValidateResponse ValidatePassword(string password)
        {
            var errorMessages = new Dictionary<string, string>();

            if (_passwordSettings.MinimumLength > password.Length)
                errorMessages.Add("MinimumLength", $"Миннимальная длинна пароля - {_passwordSettings.MinimumLength}");

            if (_passwordSettings.MaximumLength < password.Length)
                errorMessages.Add("MaximumLength", $"Максимальная длинна пароля - {_passwordSettings.MaximumLength}");

            if (_passwordSettings.IncludeLowercase)
                if (!password.Any(c => c.IsLower()))
                    errorMessages.Add("IncludeLowercase", "Пароль должен содержать строчную букву");

            if (_passwordSettings.IncludeUppercase)
                if (!password.Any(c => c.IsUpper()))
                    errorMessages.Add("IncludeUppercase", "Пароль должен содержать прописную букву");

            if (_passwordSettings.IncludeNumeric)
                if (!password.Any(c => c.IsDigit()))
                    errorMessages.Add("IncludeNumeric", "Пароль должен содержать цифру");

            if (_passwordSettings.IncludeSpecial)
                if (password.All(char.IsLetterOrDigit))
                    errorMessages.Add("IncludeSpecial", "Пароль должен символ отличный от буквы или цифры");

            var retval = new PasswordValidateResponse
            {
                Errors = errorMessages
            };

            return retval;
        }
    }
}