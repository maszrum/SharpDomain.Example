using System.Threading.Tasks;
using SharpDomain.AccessControl;
using VotingSystem.Application.Identity;

namespace VotingSystem.Application.Authorization
{
    internal class VoterMustBeAdministratorRequirement : IAuthorizationRequirement
    {
        private readonly IIdentityService<VoterIdentity> _identityService;

        public VoterMustBeAdministratorRequirement(IIdentityService<VoterIdentity> identityService)
        {
            _identityService = identityService;
        }

        public Task Handle(AuthorizationContext context)
        {
            if (_identityService.IsSignedIn)
            {
                var identity = _identityService.GetIdentity();
                if (identity.IsAdministrator)
                {
                    context.GrantAccess();
                }
            }

            return Task.CompletedTask;
        }
    }
}
