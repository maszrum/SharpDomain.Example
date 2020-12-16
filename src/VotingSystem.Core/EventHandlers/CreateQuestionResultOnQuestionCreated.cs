using System.Threading;
using System.Threading.Tasks;
using SharpDomain.Core;
using VotingSystem.Core.Events;
using VotingSystem.Core.Models;

// ReSharper disable once UnusedType.Global

namespace VotingSystem.Core.EventHandlers
{
    internal class CreateQuestionResultOnQuestionCreated : DomainEventHandler<QuestionCreated, Question>
    {
        private readonly IDomainEvents _domainEvents;

        public CreateQuestionResultOnQuestionCreated(IDomainEvents domainEvents)
        {
            _domainEvents = domainEvents;
        }

        public override Task Handle(QuestionCreated @event, Question model, CancellationToken cancellationToken)
        {
            var questionResult = QuestionResult.CreateFromQuestion(model);

            return _domainEvents
                .CollectFrom(questionResult)
                .PublishCollected(cancellationToken);
        }
    }
}