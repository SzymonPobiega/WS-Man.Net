using System;
using System.Collections.Generic;
using System.Linq;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;
using WSMan.NET.Server;
using WSMan.NET.SOAP;
using WSMan.NET.Transfer;

namespace WSMan.NET.Eventing
{
    public class EventingServer : AddressingBasedRequestHandler
    {
        private readonly FilterMap _filterMap = new FilterMap();
        private readonly Dictionary<string, Subsciption> _activeSubscriptions = new Dictionary<string, Subsciption>();
        private readonly Dictionary<HandlerMapKey, ISubscriptionManager> _enumHandlers = new Dictionary<HandlerMapKey, ISubscriptionManager>();
        private readonly EventingPullDeliveryServer _pullDeliveryServer;        

        public void BindWithPullDelivery(
           Uri listeningResourceUri,
           string dialect,
           Type filterType,
           IEventingRequestHandler eventSource,
           Uri deliveryResourceUri
           )
        {
            var enumHandler = new PullDeliverySubscriptionManager(deliveryResourceUri.ToString(), _pullDeliveryServer, eventSource);
            _filterMap.Bind(dialect, filterType);
            _enumHandlers[new HandlerMapKey(listeningResourceUri.ToString(), dialect)] = enumHandler;
        }

        protected override OutgoingMessage ProcessMessage(IncomingMessage request, ActionHeader actionHeader)
        {
            switch (actionHeader.Action)
            {
                case Constants.SubscribeAction:
                    return Subscribe(request);
                case Constants.UnsubscribeAction:
                    return Unsubscribe(request);
                case Constants.RenewAction:
                    return Renew(request);
                default:
                    return _pullDeliveryServer.Handle(request);
            }
        }

        private OutgoingMessage Subscribe(IncomingMessage request)
        {
            var selectorSetHeader = request.GetHeader<SelectorSetHeader>();
            var resourceUriHeader = request.GetHeader<ResourceUriHeader>();

            var selectors = selectorSetHeader != null 
                ? selectorSetHeader.Selectors 
                : Enumerable.Empty<Selector>();
            return Subscribe(resourceUriHeader.ResourceUri, selectors, request);
        }

        public OutgoingMessage Subscribe(string resourceUri, IEnumerable<Selector> selectors, IncomingMessage requestMessage)
        {
            var toHeader = requestMessage.GetHeader<ToHeader>();
            var subscriptionManagerReference = new EndpointReference(toHeader.Uri);
            var request = requestMessage.GetPayload<SubscribeRequest>();            
            var responseMessage = new OutgoingMessage()
                .AddHeader(new ActionHeader(Constants.SubscribeResponseAction), false);

            var expiration = request.Expires ?? Expires.FromTimeSpan(DefaultExpirationTime);

            var subsciption = GetManager(resourceUri, request.Filter)
                .Subscribe(request.Filter, selectors, expiration, subscriptionManagerReference);

            lock (_activeSubscriptions)
            {
                _activeSubscriptions[subsciption.Identifier] = subsciption;
            }

            //R7.2.4-1
            responseMessage.SetBody(
                new SerializerBodyWriter(new SubscribeResponse
                                             {
                                                 SubscriptionManager = subscriptionManagerReference,
                                                 EnumerationContext = request.Delivery.Mode == Delivery.DeliveryModePull
                                                                          ? new EnumerationContextKey(
                                                                                subsciption.Identifier)
                                                                          : null,
                                                 Expires = expiration
                                             }));

            return responseMessage;
        }

        public OutgoingMessage Unsubscribe(IncomingMessage requestMessage)
        {            
            var identifierHeader = requestMessage.GetHeader<IdentifierHeader>();

            lock (_activeSubscriptions)
            {
                Subsciption toRemove;
                if (_activeSubscriptions.TryGetValue(identifierHeader.Identifier, out toRemove))
                {
                    toRemove.Dispose();
                    _activeSubscriptions.Remove(identifierHeader.Identifier);
                }
            }
            return new OutgoingMessage()
                .AddHeader(new ActionHeader(Constants.UnsubscribeResponseAction), false);
        }

        public OutgoingMessage Renew(IncomingMessage requestMessage)
        {            
            var request = requestMessage.GetPayload<RenewRequest>();
            var identifierHeader = requestMessage.GetHeader<IdentifierHeader>();
            var responseMessage = new OutgoingMessage()
                .AddHeader(new ActionHeader(Constants.RenewResponseAction), false);

            lock (_activeSubscriptions)
            {
                Subsciption toRenew;
                if (_activeSubscriptions.TryGetValue(identifierHeader.Identifier, out toRenew))
                {
                    toRenew.Renew(request.Expires ?? Expires.FromTimeSpan(DefaultExpirationTime));
                }
            }
            responseMessage.SetBody(new SerializerBodyWriter(
                                        new RenewResponse
                                            {
                                                Expires = request.Expires
                                            }));
            return responseMessage;
        }

        private ISubscriptionManager GetManager(string resourceUri, Filter filter)
        {
            string dialect = (filter != null && filter.Dialect != null)
               ? filter.Dialect
               : FilterMap.DefaultDialect;

            //TODO: Fault is no existing
            return _enumHandlers[new HandlerMapKey(resourceUri, dialect)];
        }
        
        public EventingServer(EventingPullDeliveryServer pullDeliveryServer)
        {
            _pullDeliveryServer = pullDeliveryServer;
            DefaultExpirationTime = TimeSpan.FromHours(1);
        }

        public EventingServer()
            : this(new EventingPullDeliveryServer())
        {
        }

        public TimeSpan DefaultExpirationTime { get; set; }


    }
}