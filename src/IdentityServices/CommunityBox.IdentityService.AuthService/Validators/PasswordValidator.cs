using System.Linq;
using CommunityBox.IdentityService.AuthService.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace CommunityBox.IdentityService.AuthService.Validators
{
    public class PasswordValidator : AbstractValidator<string>
    {
        private readonly IPasswordManager _passwordManager;

        public PasswordValidator(IPasswordManager passwordManager)
        {
            _passwordManager = passwordManager;
            
            RuleFor(s => s)
                .NotEmpty();
        }

        protected override bool PreValidate(ValidationContext<string> context,
            ValidationResult result)
        {
            var password = context.InstanceToValidate;
            var validationResult = _passwordManager.ValidatePassword(password);

            if (!validationResult.Succeeded)
                result.Errors.AddRange(validationResult.Errors.Select(s =>
                    new ValidationFailure(s.Key, s.Value))
                );

            return base.PreValidate(context, result);
        }
    }
}