using System;
using VotingSystem.AccessControl.AspNetCore;
using VotingSystem.Application.Identity;
using VotingSystem.WebApi.Jwt;

namespace VotingSystem.WebApi.VoterAuthentication
{
    internal class VoterAuthenticationService : IAuthenticationService<VoterIdentity>
    {
        private readonly JwtEncryptor _jwtEncryptor;
        
        private VoterIdentity? _identity;

        public VoterAuthenticationService(JwtEncryptor jwtEncryptor)
        {
            _jwtEncryptor = jwtEncryptor;
        }

        public bool IsSignedIn => _identity is not null;
        
        public VoterIdentity GetIdentity()
        {
            if (_identity is null)
            {
                throw new InvalidOperationException(
                    "cannot get identity because voter is not signed in");
            }
            
            return _identity;
        }
        
        public void SetIdentity(VoterIdentity identity) => _identity = identity;
        
        public string GenerateToken(VoterIdentity identity) => _jwtEncryptor.GenerateToken(identity);
    }
}