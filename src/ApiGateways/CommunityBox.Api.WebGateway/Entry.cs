using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using CommunityBox.Api.WebGateway.Filters;
using CommunityBox.Api.WebGateway.Mapper.Profiles;
using CommunityBox.Api.WebGateway.Services.Abstractions;
using CommunityBox.Api.WebGateway.Services.Implementations;
using CommunityBox.Common.Swagger;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CommunityBox.Api.WebGateway
{
    public static class Entry
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton(configuration.GetSection(nameof(AuctionServiceConfig)).Get<AuctionServiceConfig>());
            services.AddSingleton(configuration.GetSection(nameof(IdentityServiceConfig)).Get<IdentityServiceConfig>());

            services.AddScoped<IAuctionService, AuctionGrpcService>();
            services.AddScoped<IIdentityService, IdentityGrpcService>();
            return services;
        }

        public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<JwtSecurityTokenHandler>();
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    RequireSignedTokens = false,
                    SignatureValidator = (token, _) =>
                        new JwtSecurityToken(token),

                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateActor = false,
                    ValidateLifetime = true
                };

                x.Events = new JwtBearerEvents
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

        public static IServiceCollection ConfigureMapper(this IServiceCollection services)
        {
            services.AddSingleton(new MapperConfiguration(mc =>
                {
                    mc.AddProfiles(new Profile[]
                    {
                        new AuctionServiceMapperProfile(),
                        new IdentityServiceMapperProfile()
                    });
                })
                .CreateMapper());

            return services;
        }

        public static IMvcBuilder RegisterFluentValidators(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            return mvcBuilder;
        }
        
        public static IServiceCollection ConfigureSwagger(
            this IServiceCollection services,
            IConfiguration configuration,
            OpenApiInfo info, params IOperationFilter[] operationFilters)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(info.Version, info);
                c.DocumentFilter<EnumDescriptionFilter>();
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Set `Bearer ` before ur `Token`",
                });

                c.OperationFilter<SwaggerAuthOperationFilter>();

                var xmlPaths =
                    AppDomain.CurrentDomain.GetAssemblies()
                        .Where(assembly => assembly.IsDefined(typeof(SwaggerIncludeXmlCommentsAttribute)))
                        .Select(GetXmlPath);

                foreach (var xmlPath in xmlPaths)
                    c.IncludeXmlComments(xmlPath);

                c.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("0.00:00:00")
                });
            });

            services.Configure<SwaggerConfig>(configuration);
            return services;
        }

        private static string GetXmlPath(Assembly assembly)
        {
            var xmlFile = $"{assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            return xmlPath;
        }
    }
}