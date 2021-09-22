using System.Threading.Tasks;
using CommunityBox.Api.WebGateway.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityBox.Api.WebGateway.Filters
{
    public class AuthorizeOnJwtSourceAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity is {IsAuthenticated: true})
            {
                var authorizationService = context.HttpContext.RequestServices.GetRequiredService<IIdentityService>();

                var isTokenValid = await authorizationService.ValidateTokenAsync(context.HttpContext.Request);

                if (!isTokenValid)
                    context.Result = new UnauthorizedResult();
            }
        }
    }
}