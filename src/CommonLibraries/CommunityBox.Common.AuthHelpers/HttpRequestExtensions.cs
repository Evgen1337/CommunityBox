using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace CommunityBox.Common.AuthHelpers
{
    public static class HttpRequestExtensions
    {
        public static string GetAuthorizationToken(this HttpRequest request) =>
            request.GetAuthorizationHeaderValue()?.Split(" ").Last();

        public static string GetAuthorizationHeaderValue(this HttpRequest request) =>
            request.Headers.TryGetValue("Authorization", out var authorizationHeader)
                ? authorizationHeader.ToString()
                : null;

        public static string GetUserIdFromAuthorizationToken(this HttpRequest request,
            JwtSecurityTokenHandler tokenHandler)
        {
            var token = request.GetAuthorizationToken();

            var jwtToken = tokenHandler.ReadJwtToken(token);
            var claim = jwtToken.Claims.SingleOrDefault(w => w.Type == JwtRegisteredClaimNames.Sub);

            return claim?.Value;
        }
    }
}