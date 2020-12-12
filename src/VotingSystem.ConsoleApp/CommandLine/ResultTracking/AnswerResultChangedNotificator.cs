using System;
using System.Collections.Generic;

namespace VotingSystem.ConsoleApp.CommandLine.ResultTracking
{
    internal class AnswerResultChangedNotificator
    {
        private readonly object _lock = new();
        private readonly List<NotificatorSubscription> _subscriptions = new();

        public void Publish(Guid answerId)
        {
            lock (_lock)
            {
                foreach (var subscription in _subscriptions)
                {
                    subscription.Next(answerId);
                }
            }
        }

        public IDisposable Subscribe(Predicate<Guid> filter, Action<Guid> onNext)
        {
            void OnUnsubscribe(NotificatorSubscription subscription)
            {
                lock (_lock)
                {
                    _subscriptions.Remove(subscription);
                }
            }

            var subscription = new NotificatorSubscription(OnUnsubscribe, onNext, filter);

            lock (_lock)
            {
                _subscriptions.Add(subscription);
            }

            return subscription;
        }
    }
}
