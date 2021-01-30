using System;
using System.Threading;
using System.Threading.Tasks;
using SharpDomain.Core;
using VotingSystem.Core.Events;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Models;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Core.EventHandlers
{
    internal class IncrementAnswerVotesOnVotePosted : DomainEventHandler<VotePosted, VoterModel>
    {
        private readonly IDomainEvents _domainEvents;
        private readonly IAnswersRepository _answersRepository;

        public IncrementAnswerVotesOnVotePosted(
            IDomainEvents domainEvents, 
            IAnswersRepository answersRepository)
        {
            _domainEvents = domainEvents;
            _answersRepository = answersRepository;
        }
        
        public override async Task Handle(VotePosted @event, VoterModel model, CancellationToken cancellationToken)
        {
            var answer = await _answersRepository.Get(@event.AnswerId);
            
            if (answer is null)
            {
                throw new NullReferenceException(
                    $"answer with id {@event.AnswerId} was not found");
            }
            
            answer.IncrementVotes();
            
            await _domainEvents
                .CollectFrom(answer)
                .PublishCollected(cancellationToken);
        }
    }
}