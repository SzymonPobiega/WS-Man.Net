using System;
using System.Collections.Generic;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing.Server
{
    public class SubscriptionCollection
    {
        private readonly NullSubscription _nullSubscription = new NullSubscription();
        private readonly Dictionary<string, ISubscription> _activeSubscriptions = new Dictionary<string, ISubscription>();

        public void Add(string identifier, Subscription subscription)
        {
            lock (_activeSubscriptions)
            {
                _activeSubscriptions[identifier] = subscription;
            }
        }

        public ISubscription Remove(string identifier)
        {
            lock (_activeSubscriptions)
            {
                ISubscription toRemove;
                if (_activeSubscriptions.TryGetValue(identifier, out toRemove))
                {
                    _activeSubscriptions.Remove(identifier);
                    return toRemove;
                }
            }
            return _nullSubscription;
        }

        public ISubscription Get(string identifier)
        {
            lock (_activeSubscriptions)
            {
                ISubscription existing;
                if (_activeSubscriptions.TryGetValue(identifier, out existing))
                {
                    return existing;
                }
            }
            return _nullSubscription;
        }

        private class NullSubscription : ISubscription
        {
            public void Dispose()
            {
            }

            public void Push(object evnt)
            {
            }

            public event EventHandler<EventPushedEventArgs> Pushed
            {
                add { }
                remove { }
            }
            public void Renew(Expires expires)
            {
            }
        }
    }
}