using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using User.Core;
using User.Core.Model;

namespace User.API.Handler
{
    public class TokenCreationHandler
    {
        private readonly IConfiguration _configuration;

        public TokenCreationHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Token CreateAccessToken(UserInfo user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new(ClaimTypes.Role, user.Role ?? string.Empty),
                new("DisplayName", user.FullName ?? string.Empty),
                new("Email", user.Email ?? string.Empty)
            };

            var expiration = DateTime.UtcNow.AddMinutes(10);
            var signingCredentials = CreateSigningCredentials();

            var jwtToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials);

            var token = new Token
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                Expiration = expiration
            };

            PopulateRefreshToken(token);
            return token;
        }

        private SigningCredentials CreateSigningCredentials()
        {
            var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT signing key is not configured.");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        private static void PopulateRefreshToken(Token token)
        {
            var number = new byte[32];
            RandomNumberGenerator.Fill(number);
            token.RefreshToken = Convert.ToBase64String(number);
        }
    }
}
