using System;
using AutoMapper;
using CommunityBox.Web.Mvc.Clients.Chat;
using CommunityBox.Web.Mvc.Clients.Gateway;
using CommunityBox.Web.Mvc.Configurations;
using CommunityBox.Web.Mvc.Mapper;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CommunityBox.Web.Mvc
{
    public static class Entry
    {
        public static IServiceCollection ConfigureClients(this IServiceCollection services,
            IConfiguration configuration)
        {
            var flurClientJsonSerializer = new NewtonsoftJsonSerializer(new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var flurClient = new FlurlClient()
                .AllowAnyHttpStatus();

            flurClient.Settings = new ClientFlurlHttpSettings
            {
                JsonSerializer = flurClientJsonSerializer
            };

            services.AddSingleton(flurClientJsonSerializer);
            services.AddSingleton<IFlurlClient, FlurlClient>(f => flurClient);

            services.AddSingleton(configuration.GetSection(nameof(GatewayClientConfig)).Get<GatewayClientConfig>());
            services.AddScoped<IGatewayClient, GatewayClient>();

            services.AddSingleton(configuration.GetSection(nameof(ChatGrpcServiceConfig))
                .Get<ChatGrpcServiceConfig>());
            services.AddSingleton<IChatGrpcService, ChatGrpcService>();
            return services;
        }
        
        public static IServiceCollection ConfigureMapper(this IServiceCollection services)
        {
            services.AddSingleton(new MapperConfiguration(mc =>
                {
                    mc.AddProfiles(new Profile[]
                    {
                        new GatewayMapperProfile(),
                        new ChatServiceMapperProfile()
                    });
                })
                .CreateMapper());

            return services;
        }

        public static IServiceCollection ConfigureAuth(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.LoginPath = new PathString("/User/Login");
                        options.AccessDeniedPath = new PathString("/Home/Index");
                        options.ExpireTimeSpan = TimeSpan.FromHours(8);
                    }
                );

            services.AddAuthorization();

            return services;
        }
    }
}