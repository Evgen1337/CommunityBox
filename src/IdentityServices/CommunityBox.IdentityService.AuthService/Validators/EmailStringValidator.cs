using FluentValidation;

namespace CommunityBox.IdentityService.AuthService.Validators
{
    public class EmailStringValidator : AbstractValidator<string>
    {
        public EmailStringValidator()
        {
            RuleFor(s => s)
                .NotEmpty()
                .EmailAddress();
        }
    }
}