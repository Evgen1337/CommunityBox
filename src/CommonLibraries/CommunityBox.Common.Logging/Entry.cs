using System;
using CommunityBox.Common.Logging.Abstractions;
using CommunityBox.Common.Logging.Implementations;
using CommunityBox.Common.Logging.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityBox.Common.Logging
{
    public static class Entry
    {
        public static IServiceCollection ConfigureLoggingServices(this IServiceCollection services)
        {
            services.AddScoped<ILoggerContext, LoggerContext>();
            services.AddScoped<ILoggerProvider, LoggerProvider>();
            services.AddScoped<ILogRequestResponseService, LogRequestResponseService>();

            return services;
        }

        public static IApplicationBuilder UseHttpRequestLogging(this IApplicationBuilder applicationBuilder) =>
            applicationBuilder.UseMiddleware<HttpRequestResponseLoggingMiddleware>();
    }
}