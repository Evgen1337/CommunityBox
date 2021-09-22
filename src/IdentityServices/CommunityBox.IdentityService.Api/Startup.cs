﻿using CommunityBox.Common.GrpcBlocks.GrpcInterceptors;
using CommunityBox.IdentityService.Api.Databases;
using CommunityBox.IdentityService.Api.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommunityBox.IdentityService.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(options => { options.Interceptors.Add<GlobalExceptionInterceptor>(); });
            services.AddAuthorization();
            services.AddAuthentication();

            services.ConfigureAuthentication(Configuration);
            services.ConfigureMapper();
            services.ConfigureIdentityDb(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<IdentityGrpcServices>();
            });

            app.ExecuteIdentityDbMigrations();
            app.SeedIdentity();
        }
    }
}