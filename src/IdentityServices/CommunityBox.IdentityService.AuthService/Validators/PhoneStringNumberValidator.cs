using System.Linq;
using FluentValidation;

namespace CommunityBox.IdentityService.AuthService.Validators
{
    /// <inheritdoc />
    public class PhoneStringNumberValidator : AbstractValidator<string>
    {
        /// <inheritdoc />
        public PhoneStringNumberValidator()
        {
            RuleFor(s => s)
                .Must(IsValidPhoneNumber)
                .WithMessage("Не валидный номер телефона");
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.SkipWhile(ch => ch != '9').Count(char.IsNumber) == 10;
        }
    }
}