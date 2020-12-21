using System.Collections.Generic;
using System.Security.Claims;

namespace VotingSystem.WebApi.Authentication
{
    internal class ClaimsProvider
    {
        public IEnumerable<Claim> GetFor<TIdentity>(TIdentity identity) 
            where TIdentity : class
        {
            // TODO
            return null;
        }
    }
}