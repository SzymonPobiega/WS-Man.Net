using System;
using System.Collections.Generic;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;
using WSMan.NET.Server;
using WSMan.NET.SOAP;

namespace WSMan.NET.Eventing
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
        /// <param name="subscriptionResourceUri">URI of resource representing event source.</param>
        /// <param name="pullResourceUri">URI of resource to poll for notifications.</param>
        /// <param name="filter">Optional object used to filter event stream on the server.</param>
        /// <param name="selectors">Optional collection of selector objects which provide
        /// additional description of requested event stream.</param>
        /// <returns></returns>
        public IDisposable SubscribeUsingPullDelivery<T>(Action<T> callback, bool synchronizeCallbackThread, string subscriptionResourceUri, string pullResourceUri, Filter filter, params Selector[] selectors)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback", "Callback method must be specified.");
            }
            if (subscriptionResourceUri == null)
            {
                throw new ArgumentNullException("subscriptionResourceUri", "URI of resource representing event source must be specified.");
            }
            if (pullResourceUri == null)
            {
                throw new ArgumentNullException("pullResourceUri", "URI of resource to poll for notifications must be specified.");
            }
            var impl = SubscribeUsingPullDelivery<T>(subscriptionResourceUri, pullResourceUri, filter, (IEnumerable<Selector>)selectors);
            return new CallbackThreadPoolPullSubscriptionClient<T>(callback, impl, synchronizeCallbackThread);
        }

        /// <summary>
        /// Subscribes to the event stream using PULL delivery mode. Events will be processed
        /// by provided callback using a ThreadPool thread.
        /// </summary>
        /// <typeparam name="T">Type to which event messages should be deserialized.</typeparam>
        /// <param name="callback">Callback method used to process events.</param>
        /// <param name="subscriptionResourceUri">URI of resource representing event source.</param>
        /// <param name="pullResourceUri">URI of resource to poll for notifications.</param>
        /// <param name="filter">Optional object used to filter event stream on the server.</param>
        /// <param name="selectors">Optional collection of selector objects which provide
        /// additional description of requested event stream.</param>
        /// <returns></returns>
        public IDisposable SubscribeUsingPullDelivery<T>(Action<T> callback, string subscriptionResourceUri, string pullResourceUri, Filter filter, params Selector[] selectors)
        {
            return SubscribeUsingPullDelivery(callback, true, subscriptionResourceUri, pullResourceUri, filter, selectors);
        }

        public IPullSubscriptionClient<T> SubscribeUsingPullDelivery<T>(string subscriptionResourceUri, string pullResourceUri, Filter filter, params Selector[] selectors)
        {
            return SubscribeUsingPullDelivery<T>(subscriptionResourceUri, pullResourceUri, filter, (IEnumerable<Selector>)selectors);
        }

        public IPullSubscriptionClient<T> SubscribeUsingPullDelivery<T>(string subscriptionResourceUri, string pullResourceUri, Filter filter, IEnumerable<Selector> selectors)
        {
            var responseMessage = _soapClient.BuildMessage()
                .WithAction(Constants.SubscribeAction)
                .WithResourceUri(subscriptionResourceUri)
                .AddBody(new SubscribeRequest
                             {
                                 Delivery = Delivery.Pull(),
                                 Filter = filter
                             })
                .SendAndGetResponse();

            var response = responseMessage.GetPayload<SubscribeResponse>();
            return new PullSubscriptionClientImpl<T>(_soapClient, response.EnumerationContext, pullResourceUri);
        }
    }
}