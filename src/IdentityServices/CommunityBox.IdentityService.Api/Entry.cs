using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommunityBox.IdentityService.AuthService;
using CommunityBox.IdentityService.AuthService.Configurations;
using CommunityBox.IdentityService.Api.Databases;
using CommunityBox.IdentityService.Api.Domain.Entities;
using CommunityBox.IdentityService.Api.Mapper;
using CommunityBox.IdentityService.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CommunityBox.IdentityService.Api
{
    public static class Entry
    {
        public static IServiceCollection ConfigureIdentityDb(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContextPool<IdentityContext>(opt =>
                opt.UseNpgsql(
                    configuration.GetConnectionString("IdentityDbConnection")
                )
            );

            services.AddScoped<IdentityUserManager>();
            services.AddIdentityCore<User>(opts =>
                {
                    opts.Password.RequireDigit = true;
                    opts.Password.RequireLowercase = true;
                    opts.Password.RequireUppercase = true;
                    opts.Password.RequireNonAlphanumeric = true;
                    opts.User.RequireUniqueEmail = true;
                    opts.User.AllowedUserNameCharacters = null;
                })
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<IdentityContext>();

            return services;
        }
        
        public static IServiceCollection ConfigureMapper(this IServiceCollection services)
        {
            services.AddSingleton(new MapperConfiguration(mc =>
                {
                    mc.AddProfiles(new Profile[]
                    {
                        new EntityMapperProfile()
                    });
                })
                .CreateMapper());

            return services;
        }
        
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection(nameof(JwtConfiguration)).Get<JwtConfiguration>();

            services.ConfigureJwtAuthService(jwtConfig);

            services.AddSingleton<JwtSecurityTokenHandler>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero
            };
            
            services.AddSingleton(tokenValidationParameters);
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    opt =>
                    {
                        opt.TokenValidationParameters = tokenValidationParameters;

                        opt.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                    context.Response.Headers.Add("Token-Expired", "true");

                                return Task.CompletedTask;
                            }
                        };
                    });

            return services;
        }
        
        public static void ExecuteIdentityDbMigrations(this IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<IdentityContext>();

            if (context.Database.IsInMemory())
                return;

            //logger.LogInfoObj($"{nameof(IdentityContext)} migrations executing", "MigrationsExecuting");
            context.Database.Migrate();
            //logger.LogInfoObj($"{nameof(IdentityContext)} migrations executed", "MigrationsExecuted");
        }
    }
}