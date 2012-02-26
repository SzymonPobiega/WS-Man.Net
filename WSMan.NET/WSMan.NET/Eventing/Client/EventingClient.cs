using System;
using System.Collections.Generic;
using WSMan.NET.Enumeration;
using WSMan.NET.Server;
using WSMan.NET.SOAP;

namespace WSMan.NET.Eventing.Client
{
    public class EventingClient
    {
        private readonly ISOAPClient _soapClient;
        private readonly FilterMap _filterMap = new FilterMap();        

        public EventingClient(string serverUrl)
            : this(new SOAPClient(serverUrl))
        {
        }

        public EventingClient(ISOAPClient soapClient)
        {
            _soapClient = soapClient;
        }

        public EventingClient BindFilterDialect(string dialect, Type implementationType)
        {
            _filterMap.Bind(dialect, implementationType);
            return this;
        }

        /// <summary>
        /// Subscribes to the event stream using PULL delivery mode. Events will be processed
        /// by provided callback using a ThreadPool thread.
        /// </summary>
        /// <typeparam name="T">Type to which event messages should be deserialized.</typeparam>
        /// <param name="callback">Callback method used to process events.</param>
        /// <param name="synchronizeCallbackThread">If true, when closing subscription current thread will block until last PULL request ends.</param>
        /// <param name="filter">Optional object used to filter event stream on the server.</param>
        /// <param name="additonalHeaders"></param>
        /// <returns></returns>
        public IDisposable SubscribeUsingPullDelivery<T>(Action<T> callback, bool synchronizeCallbackThread, Filter filter, params IMessageHeaderWithMustUnderstandSpecification[] additonalHeaders)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback", "Callback method must be specified.");
            }            
            var impl = SubscribeUsingPullDelivery<T>(filter, additonalHeaders);
            return new CallbackThreadPoolPullSubscriptionClient<T>(callback, impl, synchronizeCallbackThread);
        }

        /// <summary>
        /// Subscribes to the event stream using PULL delivery mode. Events will be processed
        /// by provided callback using a ThreadPool thread.
        /// </summary>
        /// <typeparam name="T">Type to which event messages should be deserialized.</typeparam>
        /// <param name="callback">Callback method used to process events.</param>
        /// <param name="filter">Optional object used to filter event stream on the server.</param>
        /// <param name="additonalHeaders"></param>
        /// <returns></returns>
        public IDisposable SubscribeUsingPullDelivery<T>(Action<T> callback, Filter filter, params IMessageHeaderWithMustUnderstandSpecification[] additonalHeaders)
        {
            return SubscribeUsingPullDelivery(callback, true, filter, additonalHeaders);
        }

        public IPullSubscriptionClient<T> SubscribeUsingPullDelivery<T>(Filter filter, params IMessageHeaderWithMustUnderstandSpecification[] additonalHeaders)
        {
            var context = _soapClient
                .BuildMessage()
                .AddHeaders(additonalHeaders)
                .SubscribeUsingPullDelivery(filter);            

            return new PullSubscriptionClientImpl<T>(_soapClient, context, additonalHeaders);
        }
    }
}