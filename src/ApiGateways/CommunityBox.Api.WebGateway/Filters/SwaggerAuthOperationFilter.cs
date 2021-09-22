using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CommunityBox.Api.WebGateway.Filters
{
    public class SwaggerAuthOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context != null)
            {
                var authAttributes = context.MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Distinct();

                var customAuthAttributes = context.MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeOnJwtSourceAttribute>()
                    .Distinct();

                var attributes = authAttributes.Concat(customAuthAttributes);

                if (!attributes.Any())
                    return;
            }

            if (operation == null) 
                return;
            
            operation.Responses.TryAdd("401", new OpenApiResponse {Description = "Unauthorized"});

            var bearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "bearer"}
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [bearerScheme] = Array.Empty<string>()
                }
            };
        }
    }
}