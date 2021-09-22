using Autofac;
using CommunityBox.AuctionService.Api.Application.Queries;
using CommunityBox.AuctionService.Api.AutofacModules;
using CommunityBox.AuctionService.Api.Grpc;
using CommunityBox.Common.GrpcBlocks.GrpcInterceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommunityBox.AuctionService.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureContainer(ContainerBuilder builder) => builder.RegisterModule<IocConfigModule>();

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(options => { options.Interceptors.Add<GlobalExceptionInterceptor>(); });
            
            services.ConfigureKafka(Configuration);
            services.AddScoped<IAuctionQueries, AuctionQueries>();
            services.ConfigureMapper();
            services.ConfigureAuctionDb(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<AuctionGrpcService>();
            });

            app.ExecuteCardsDbMigrations();
        }
    }
}