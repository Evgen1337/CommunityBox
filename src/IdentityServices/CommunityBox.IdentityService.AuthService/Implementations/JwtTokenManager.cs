using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using CommunityBox.Common.AuthHelpers;
using CommunityBox.IdentityService.AuthService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace CommunityBox.IdentityService.AuthService.Implementations
{
    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtTokenManager(IJwtGenerator jwtGenerator, JwtSecurityTokenHandler tokenHandler,
            TokenValidationParameters tokenValidationParameters)
        {
            _jwtGenerator = jwtGenerator;
            _tokenHandler = tokenHandler;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public string GenerateToken(string email, string userId)
        {
            return _jwtGenerator.CreateToken(email, userId);
        }

        public string RefreshToken(string token, string email, string userId)
        {
            return _jwtGenerator.RefreshToken(token, email, userId);
        }

        public string GetUserIdFromHeaderToken(HttpRequest request)
        {
            var token = request.GetAuthorizationToken();

            var jwtToken = _tokenHandler.ReadJwtToken(token);
            var claim = jwtToken.Claims.SingleOrDefault(w => w.Type == JwtRegisteredClaimNames.Sub);

            return claim?.Value;
        }

        public bool ValidateToken(string token)
        {
            try
            {
                _tokenHandler.ValidateToken(token, _tokenValidationParameters, out _);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}