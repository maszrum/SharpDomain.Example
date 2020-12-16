using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SharpDomain.Core;
using VotingSystem.Core.Events;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Models;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Core.EventHandlers
{
    internal class UpdateAnswerResultOnVotePosted : DomainEventHandler<VotePosted, Vote>
    {
        private readonly IDomainEvents _domainEvents;
        private readonly IAnswerResultsRepository _answerResultsRepository;

        public UpdateAnswerResultOnVotePosted(
            IDomainEvents domainEvents, 
            IAnswerResultsRepository answerResultsRepository)
        {
            _domainEvents = domainEvents;
            _answerResultsRepository = answerResultsRepository;
        }

        public override async Task Handle(VotePosted @event, Vote model, CancellationToken cancellationToken)
        {
            var answerResult = await _answerResultsRepository.GetAnswerResultByAnswerId(@event.AnswerId);
            
            if (answerResult is null)
            {
                throw new NullReferenceException(
                    $"answer result associated with answer id {@event.AnswerId} was not found");
            }
            
            answerResult.IncrementVotes();
            
            await _domainEvents
                .CollectFrom(answerResult)
                .PublishCollected(cancellationToken);
        }
    }
}