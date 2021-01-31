using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SharpDomain.AccessControl;
using SharpDomain.AccessControl.AspNetCore;

namespace VotingSystem.WebApi.Jwt
{
    internal class JwtEncryptor
    {
        private readonly JwtConfiguration _configuration;
        private readonly IClaimsIdentityConverter _claimsIdentityConverter;
        
        private readonly byte[] _secretCached;

        public JwtEncryptor(
            JwtConfiguration configuration,
            IClaimsIdentityConverter claimsProvider)
        {
            _configuration = configuration;
            _claimsIdentityConverter = claimsProvider;
            
            _secretCached = Encoding.UTF8.GetBytes(configuration.Secret);
        }
        
        public string GenerateToken<TIdentity>(TIdentity identity) 
            where TIdentity : IIdentity
        {
            var claims = _claimsIdentityConverter.GetClaims(identity);
            
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
