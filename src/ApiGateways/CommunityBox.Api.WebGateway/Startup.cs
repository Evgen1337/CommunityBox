using CommunityBox.Api.WebGateway.Convertors;
using CommunityBox.Common.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace CommunityBox.Api.WebGateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureServices(Configuration);
            services.ConfigureMapper();
            services.ConfigureJwtAuthentication(Configuration);
            services.ConfigureSwagger(Configuration, new OpenApiInfo
            {
                Title = "CommunityBox.Api.WebGateway",
                Version = "v1"
            });

            services.AddMvc()
                .AddNewtonsoftJson()
                .RegisterFluentValidators();

            services.AddControllers()
                .AddJsonOptions(
                    options => options.JsonSerializerOptions.Converters.Add(new TimeSpanToStringConverter()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<SwaggerConfig> swaggerOptions)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            if (swaggerOptions.Value.UseSwagger)
                app.UseConfiguredSwagger();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}