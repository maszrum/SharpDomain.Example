using System;
using SharpDomain.AccessControl;
using VotingSystem.Application.Identity;

// ReSharper disable once ClassNeverInstantiated.Global

namespace VotingSystem.ConsoleApp
{
    internal class AuthenticationService : IIdentityService<VoterIdentity>
    {
        private VoterIdentity? _identity;

        public bool IsSignedIn => _identity is not null;

        public VoterIdentity GetIdentity()
        {
            if (_identity is null)
            {
                throw new InvalidOperationException(
                    "voter is not signed in");
            }

            return _identity;
        }

        public void SetIdentity(VoterIdentity identity) => 
            _identity = identity;

        public void ResetIdentity() => 
            _identity = default;
    }
}