using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CommunityBox.Web.Mvc.Middlewares
{
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializerSettings _serializerSettings;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
            _serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                /*var logger = loggerProvider.GetLogger<GlobalExceptionMiddleware>();
                logger.LogUnhandledErrorObj(ex, loggerProvider.LoggerContext);*/
                
                await HandleExceptionAsync(context, ex);
            }
        }
        
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            /*var statusCode = exception is ClientExceptionBase ex
                ? ex.StatusCode
                : HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;

            var jsonResponseContainer = new JsonResponseContainer
            {
                Errors = new[]
                {
                    new JsonResponseError
                    {
                        Title = statusCode.ToString("G"),
                        Detail = exception.Message
                    }
                }
            };

            var json = JsonConvert.SerializeObject(jsonResponseContainer, _serializerSettings);*/
            return context.Response.WriteAsync(exception.Message);
        }
    }
}