using System.Threading;
using System.Threading.Tasks;
using SharpDomain.Application;
using SharpDomain.Core;
using SharpDomain.Errors;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Models;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Application.Commands
{
    internal class VoteForHandler : CommandHandler<VoteFor>
    {
        private readonly IDomainEvents _domainEvents;
        private readonly IVotersRepository _votersRepository;

        public VoteForHandler(
            IDomainEvents domainEvents, 
            IVotersRepository votersRepository)
        {
            _domainEvents = domainEvents;
            _votersRepository = votersRepository;
        }

        public override async Task<Response<Empty>> Handle(VoteFor request, CancellationToken cancellationToken)
        {
            var voter = await _votersRepository.Get(request.VoterId);
            if (voter is null)
            {
                return ObjectNotFoundError.CreateFor<Voter>(request.VoterId);
            }
            
            var vote = voter.Vote(
                request.VoterId, 
                request.QuestionId, 
                request.AnswerId);
            
            await _domainEvents
                .CollectFrom(vote)
                .CollectFrom(voter)
                .PublishCollected(cancellationToken);
            
            return Nothing();
        }
    }
}