using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityBox.IdentityService.Api.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CommunityBox.IdentityService.Api.Services
{
    public class IdentityUserManager : UserManager<User>
    {
        public IdentityUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) : base(
            store, optionsAccessor, passwordHasher, Enumerable.Empty<IUserValidator<User>>(), passwordValidators,
            keyNormalizer, errors, services,
            logger)
        {
            Options.User.AllowedUserNameCharacters = null;
        }

        public async Task<bool> IsExistByPhoneAsync(string phoneNumber) =>
            await Users.AnyAsync(a => a.PhoneNumber == phoneNumber);

        public async Task<bool> IsExistByEmailAsync(string email)
        {
            var normalizedEmail = NormalizeEmail(email);
            return await Users.AnyAsync(a => a.NormalizedEmail == normalizedEmail);
        }

        public async Task<User> FindFullInfoUserByIdAsync(string id) =>
            await Users
                .Include(i => i.AccountSetting)
                .Include(i => i.UserPersonalInformation)
                .FirstOrDefaultAsync(f => f.Id == id);
    }
}