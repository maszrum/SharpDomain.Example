using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace VotingSystem.WebApi.Authentication
{
    internal class JwtEncryptor
    {
        private readonly JwtConfiguration _configuration;
        private readonly ClaimsProvider _claimsProvider;
        
        private readonly byte[] _secretCached;

        public JwtEncryptor(
            JwtConfiguration configuration, 
            ClaimsProvider claimsProvider)
        {
            _configuration = configuration;
            _claimsProvider = claimsProvider;
            
            _secretCached = Encoding.UTF8.GetBytes(configuration.Secret);
        }
        
        public string GenerateToken<TIdentity>(TIdentity identity) 
            where TIdentity : class
        {
            var claims = _claimsProvider.GetFor(identity);
            
            var signingCredentials = new SigningCredentials(
                key: new SymmetricSecurityKey(_secretCached), 
                algorithm: SecurityAlgorithms.HmacSha256Signature);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.ExpiresInMinutes),
                SigningCredentials = signingCredentials
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }
    }
}
