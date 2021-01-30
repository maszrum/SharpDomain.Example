using System.Threading.Tasks;
using SharpDomain.AccessControl;
using VotingSystem.Application.Identity;

// ReSharper disable once ClassNeverInstantiated.Global

namespace VotingSystem.Application.Authorization
{
    internal class VoterMustBeLoggedInRequirement : IAuthorizationRequirement
    {
        private readonly IIdentityService<VoterIdentity> _identityService;

        public VoterMustBeLoggedInRequirement(IIdentityService<VoterIdentity> identityService)
        {
            _identityService = identityService;
        }

        public Task Handle(AuthorizationContext context)
        {
            if (_identityService.IsSignedIn)
            {
                context.GrantAccess();
            }

            return Task.CompletedTask;
        }
    }
}
