using System.Linq;
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
        private readonly IVotesRepository _votesRepository;

        public VoteForHandler(
            IDomainEvents domainEvents, 
            IVotesRepository votesRepository)
        {
            _domainEvents = domainEvents;
            _votesRepository = votesRepository;
        }

        public override async Task<Response<Empty>> Handle(VoteFor request, CancellationToken cancellationToken)
        {
            var voterVotes = await _votesRepository.GetByVoter(request.VoterId);

            var alreadyVoted = voterVotes.Any(v => v.QuestionId == request.QuestionId);
            if (alreadyVoted)
            {
                return new UserError("this question has already been voted");
            }

            var vote = Vote.Create(request.VoterId, request.QuestionId, request.AnswerId);

            await _domainEvents
                .CollectFrom(vote)
                .PublishCollected(cancellationToken);
            
            var not1 = base.Nothing();
            var not2 = this.Nothing();
            var not3 = Nothing();
            
            return Nothing();
        }
    }
}