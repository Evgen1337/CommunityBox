﻿using CommunityBox.ChatService.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommunityBox.ChatService.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();

            services.AddScoped<IChatRepository, ChatRepository>();
            services.ConfigureKafka(Configuration);
            services.ConfigureClients(Configuration);
            services.ConfigureAuctionDb(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapGrpcService<ChatGrpcServices>(); });

            app.ExecuteCardsDbMigrations();
        }
    }
}