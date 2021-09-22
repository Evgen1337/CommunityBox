using System;
using System.Linq;
using CommunityBox.Common.Core;
using CommunityBox.Common.Core.SystemChat;
using CommunityBox.IdentityService.Api.Domain.Entities;
using CommunityBox.IdentityService.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityBox.IdentityService.Api.Databases
{
    public static class SeedIdentityContext
    {
        public static void SeedIdentity(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var userManager = serviceProvider.GetRequiredService<IdentityUserManager>();

            if (userManager.Users.Any())
                return;

            var systemUser = new User
            {
                Id = SystemChatSettings.SystemUserId,
                Email = "string",
                UserName = SystemChatSettings.SystemUserName,
                PhoneNumber = "string",
                EmailConfirmed = true,
                UserPersonalInformation = new UserPersonalInformation
                {
                    FirstName = "test",
                    BirthDay = DateTime.Now,
                    LastName = "test",
                },
                AccountSetting = new AccountSetting
                {
                    ShowBirthDay = false,
                    ShowEmail = false,
                    ShowPhone = false
                }
            };

            userManager.CreateAsync(systemUser, "Qwerty123!").GetAwaiter().GetResult();

            var user = new User
            {
                Email = "string",
                UserName = "testtestname",
                PhoneNumber = "testnumber",
                EmailConfirmed = true,
                UserPersonalInformation = new UserPersonalInformation
                {
                    FirstName = "test",
                    BirthDay = DateTime.Now,
                    LastName = "test",
                },
                AccountSetting = new AccountSetting()
            };


            for (var i = 0; i < 10; i++)
            {
                var newUser = new User
                {
                    Email = $"{user.Email}{i}",
                    UserName = $"{user.Email}{i}",
                    PhoneNumber = $"{user.Email}{i}",
                    EmailConfirmed = true,
                    UserPersonalInformation = new UserPersonalInformation
                    {
                        FirstName = $"{user.Email}{i}",
                        BirthDay = DateTime.Now,
                        LastName = $"{user.Email}{i}",
                    },
                    AccountSetting = new AccountSetting()
                };

                userManager.CreateAsync(newUser, "Qwerty123!").GetAwaiter().GetResult();
            }
        }
    }
}