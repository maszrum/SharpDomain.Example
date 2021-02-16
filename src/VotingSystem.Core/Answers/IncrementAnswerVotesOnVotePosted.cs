using System;
using System.Threading;
using System.Threading.Tasks;
using SharpDomain.Core;
using VotingSystem.Core.InfrastructureAbstractions;
using VotingSystem.Core.Voters;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Core.Answers
{
    internal class IncrementAnswerVotesOnVotePosted : IEventHandler<VotePosted>
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

        public async Task Handle(VotePosted notification, CancellationToken cancellationToken)
        {
            var answer = await _answersRepository.Get(notification.AnswerId)!;
            
            if (answer is null)
            {
                throw new NullReferenceException(
                    $"answer with id {notification.AnswerId} was not found");
            }
            
            answer.IncrementVotes();
            
            await _domainEvents
                .CollectFrom(answer)
                .PublishCollected(cancellationToken);
        }
    }
}