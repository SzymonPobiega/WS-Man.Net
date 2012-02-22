using System;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration;
using WSMan.NET.Server;
using WSMan.NET.SOAP;
using WSMan.NET.Transfer;

namespace WSMan.NET.Eventing.Server
{
    public class EventingServer : AddressingBasedRequestHandler
    {
        private readonly IEventingRequestHandler _requestHandler;
        private readonly FilterMap _filterMap = new FilterMap();
        private readonly SubscriptionCollection _activeSubscriptions = new SubscriptionCollection();

        public TimeSpan DefaultExpirationTime { get; set; }

        public event EventHandler<SubscribedEventArgs> Subscribed;
        public event EventHandler<UnsubscribedEventArgs> Unsubscribed;

        public EventingServer Bind(string dialect, Type filterType)
        {
            _filterMap.Bind(dialect, filterType);
            return this;
        }

        public EventingServer(IEventingRequestHandler requestHandler)
        {
            _requestHandler = requestHandler;
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
                    return null;
            }
        }

        private OutgoingMessage Subscribe(IncomingMessage requestMessage)
        {
            var toHeader = requestMessage.GetHeader<ToHeader>();
            var subscriptionManagerReference = new EndpointReference(toHeader.Uri);
            var request = requestMessage.GetPayload<SubscribeRequest>();            
            var responseMessage = new OutgoingMessage()
                .AddHeader(new ActionHeader(Constants.SubscribeResponseAction), false);

            var expiration = CalculateExpiration(request.Expires);
            var filterInstance = GetFilterInstance(request);
            var identifier = GenerateUniqueSubscriptionIdentifier();
            var subscription = new Subscription(expiration, _requestHandler, filterInstance, subscriptionManagerReference, requestMessage);
            _activeSubscriptions.Add(identifier, subscription);
            OnSubscribed(identifier, subscription);

            //R7.2.4-1
            var body = new SubscribeResponse
                           {
                               SubscriptionManager = subscriptionManagerReference,
                               EnumerationContext = request.Delivery.Mode == Delivery.DeliveryModePull
                                                        ? new EnumerationContextKey(
                                                              identifier)
                                                        : null,
                               Expires = expiration
                           };
            responseMessage.SetBody(new SerializerBodyWriter(body));
            return responseMessage;
        }

        private object GetFilterInstance(SubscribeRequest request)
        {
            if (request.Filter != null)
            {
                var filterType = _filterMap.GetFilterType(request.Filter.Dialect);
                return request.Filter.DeserializeAs(filterType);
            }
            return null;
        }

        private static string GenerateUniqueSubscriptionIdentifier()
        {
            return Guid.NewGuid().ToString("N");
        }

        private void OnSubscribed(string identifier, Subscription subscription)
        {
            if (Subscribed != null)
            {
                Subscribed(this, new SubscribedEventArgs(subscription, identifier));
            }
        }

        public OutgoingMessage Unsubscribe(IncomingMessage requestMessage)
        {            
            var identifierHeader = requestMessage.GetHeader<IdentifierHeader>();
            var identifier = identifierHeader.Identifier;

            var toRemove = _activeSubscriptions.Remove(identifier);
            OnUnsubscribed(identifier, toRemove);
            toRemove.Dispose();

            return new OutgoingMessage()
                .AddHeader(new ActionHeader(Constants.UnsubscribeResponseAction), false);
        }

        private void OnUnsubscribed(string identifier, ISubscription toRemove)
        {
            if (Unsubscribed != null)
            {
                Unsubscribed(this, new UnsubscribedEventArgs(toRemove, identifier));
            }
        }

        public OutgoingMessage Renew(IncomingMessage requestMessage)
        {            
            var request = requestMessage.GetPayload<RenewRequest>();
            var identifierHeader = requestMessage.GetHeader<IdentifierHeader>();
            

            var existing = _activeSubscriptions.Get(identifierHeader.Identifier);
            existing.Renew(CalculateExpiration(request.Expires));

            var responseMessage = new OutgoingMessage()
                .AddHeader(new ActionHeader(Constants.RenewResponseAction), false);
            responseMessage.SetBody(new SerializerBodyWriter(
                                        new RenewResponse
                                            {
                                                Expires = request.Expires
                                            }));
            return responseMessage;
        }

        private Expires CalculateExpiration(Expires nullableExpirationValue)
        {
            return nullableExpirationValue ?? Expires.FromTimeSpan(DefaultExpirationTime);
        }
    }
}