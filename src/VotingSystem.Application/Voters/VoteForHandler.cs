using System;
using System.Threading;
using System.Threading.Tasks;
using SharpDomain.AccessControl;
using SharpDomain.Application;
using SharpDomain.Core;
using SharpDomain.Responses;
using VotingSystem.Application.Authorization;
using VotingSystem.Application.Identity;
using VotingSystem.Core.InfrastructureAbstractions;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Voters
{
    internal class VoteForHandler : CommandHandler<VoteFor>, IAuthorizationRequired
    {
        private readonly IDomainEvents _domainEvents;
        private readonly IVotersRepository _votersRepository;
        private readonly IIdentityService<VoterIdentity> _identityService;

        public VoteForHandler(
            IDomainEvents domainEvents, 
            IVotersRepository votersRepository, 
            IIdentityService<VoterIdentity> identityService)
        {
            _domainEvents = domainEvents;
            _votersRepository = votersRepository;
            _identityService = identityService;
        }

        public void ConfigureAuthorization(AuthorizationConfiguration configuration) =>
            configuration.UseRequirement<VoterMustBeLoggedInRequirement>();

        public override async Task<Response<Empty>> Handle(VoteFor request, CancellationToken cancellationToken)
        {
            var identity = _identityService.GetIdentity();
            
            var voter = await _votersRepository.Get(identity.Id);
            if (voter is null)
            {
                throw new NullReferenceException(
                    "cannot find voter with specified id");
            }
            
            var vote = voter.Vote(
                voter.Id, 
                request.QuestionId, 
                request.AnswerId);
            
            await _domainEvents
                .CollectFrom(vote)
                .CollectFrom(voter)
                .PublishCollected(cancellationToken);
            
            return Success();
        }
    }
}