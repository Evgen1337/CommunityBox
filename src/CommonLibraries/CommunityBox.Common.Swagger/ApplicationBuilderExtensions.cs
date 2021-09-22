using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

namespace CommunityBox.Common.Swagger
{
    /// <summary>
    ///     Предоставляет методы расширений для типа <see cref="IApplicationBuilder" />.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        private const string SwaggerPath = "/swagger/v1/swagger.json";

        /// <summary>
        ///     Включает сконфигурированную обработку Swagger-запросов.
        /// </summary>
        /// <param name="app">
        ///     Экземпляр объекта, предоставляющего механизмы конфигурирования каналов обработки запросов приложения.
        /// </param>
        public static void UseConfiguredSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers = new List<OpenApiServer>
                        {new OpenApiServer {Url = $"{httpReq.Scheme}://{httpReq.Host.Value}"}};
                });
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c => { c.SwaggerEndpoint(SwaggerPath, "Api V1"); });
        }
    }
}