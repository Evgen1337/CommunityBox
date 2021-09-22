using System.Linq;
using Flurl.Http;
using Microsoft.AspNetCore.Http;

namespace CommunityBox.Web.Mvc.Clients.Gateway
{
    public static class GatewayClientExtensions
    {
        public static IFlurlRequest SetAuthHeaderFromContext(this IFlurlRequest request, HttpContext httpContext)
        {
            var authTokenClaim = httpContext.User.Claims.FirstOrDefault(f => f.Type == "token");

            if (authTokenClaim == null)
                return request;

            var authToken = authTokenClaim.Value;

            request.WithHeader("Authorization", $"Bearer {authToken}");
            return request;
        }
    }
}