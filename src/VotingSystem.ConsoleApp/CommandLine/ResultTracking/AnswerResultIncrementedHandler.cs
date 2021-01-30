using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VotingSystem.Core.Answer;

namespace VotingSystem.ConsoleApp.CommandLine.ResultTracking
{
    internal class AnswerResultIncrementedHandler : INotificationHandler<AnswerVotesIncremented>
    {
        private readonly AnswerResultChangedNotificator _notificator;

        public AnswerResultIncrementedHandler(AnswerResultChangedNotificator notificator)
        {
            _notificator = notificator;
        }

        public Task Handle(AnswerVotesIncremented notification, CancellationToken cancellationToken)
        {
            _notificator.Publish(notification.AnswerId);

            return Task.CompletedTask;
        }
    }
}
