using System;

namespace VotingSystem.ConsoleApp.CommandLine.ResultTracking
{
    internal class NotificatorSubscription : IDisposable
    {
        public NotificatorSubscription(
            Action<NotificatorSubscription> onUnsubscribe, 
            Action<Guid> action, 
            Predicate<Guid>? filter)
        {
            _onUnsubscribe = onUnsubscribe;
            _action = action;
            _filter = filter;
        }

        private readonly Action<NotificatorSubscription> _onUnsubscribe;
        private readonly Action<Guid> _action;
        private readonly Predicate<Guid>? _filter;

        public void Next(Guid id)
        {
            if (_filter == default || _filter(id))
            {
                _action(id);
            }
        }

        public void Dispose()
        {
            _onUnsubscribe(this);
        }
    }
}
