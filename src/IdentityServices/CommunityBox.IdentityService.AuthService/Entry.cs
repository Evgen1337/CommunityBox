using CommunityBox.IdentityService.AuthService.Configurations;
using CommunityBox.IdentityService.AuthService.Implementations;
using CommunityBox.IdentityService.AuthService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PasswordGenerator;

namespace CommunityBox.IdentityService.AuthService
{
    public static class Entry
    {
        public static IServiceCollection ConfigureJwtAuthService(this IServiceCollection services,
            JwtConfiguration jwtConfiguration)
        {
            services.AddSingleton(jwtConfiguration);
            
            services.AddSingleton<IPasswordSettings>(s =>
                new CustomPasswordSettings(true, true, true, true, 16, int.MaxValue, false));
            services.AddSingleton<IPasswordManager, PasswordManager>();

            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IJwtTokenManager, JwtTokenManager>();

            return services;
        }
    }
}