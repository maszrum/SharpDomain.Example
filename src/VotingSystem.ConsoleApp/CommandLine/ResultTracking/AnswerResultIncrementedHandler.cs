using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VotingSystem.Core.Events;

namespace VotingSystem.ConsoleApp.CommandLine.ResultTracking
{
    internal class AnswerResultIncrementedHandler : INotificationHandler<AnswerResultIncremented>
    {
        private readonly AnswerResultChangedNotificator _notificator;

        public AnswerResultIncrementedHandler(AnswerResultChangedNotificator notificator)
        {
            _notificator = notificator;
        }

        public Task Handle(AnswerResultIncremented notification, CancellationToken cancellationToken)
        {
            _notificator.Publish(notification.AnswerId);

            return Task.CompletedTask;
        }
    }
}
