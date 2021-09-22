using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CommunityBox.IdentityService.AuthService.Configurations;
using CommunityBox.IdentityService.AuthService.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CommunityBox.IdentityService.AuthService.Implementations
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly SigningCredentials _credentials;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public JwtGenerator(JwtConfiguration options, JwtSecurityTokenHandler tokenHandler)
        {
            _jwtConfiguration = options;
            _tokenHandler = tokenHandler;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key));
            _credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        }

        public string CreateToken(string email, string userId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Sub, userId)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtConfiguration.LifeTime),
                SigningCredentials = _credentials
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }

        public string RefreshToken(string token, string email, string userId)
        {
            if (!_tokenHandler.CanReadToken(token))
                throw new SecurityTokenValidationException();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Sub, userId)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtConfiguration.LifeTime),
                SigningCredentials = _credentials
            };

            var newToken = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(newToken);
        }
    }
}