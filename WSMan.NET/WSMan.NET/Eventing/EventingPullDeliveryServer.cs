using System;
using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration;
using WSMan.NET.Enumeration.Faults;
using WSMan.NET.Management.Faults;
using WSMan.NET.Server;
using WSMan.NET.SOAP;
using WSMan.NET.Transfer;

namespace WSMan.NET.Eventing
{    
    public class EventingPullDeliveryServer : AddressingBasedRequestHandler
    {
        private readonly Dictionary<string, PullSubscription> _subscriptions = new Dictionary<string, PullSubscription>();

        public void AddSubscription(PullSubscription subscription)
        {
            _subscriptions[subscription.Identifier] = subscription;
        }

        public void RemoveSubscription(PullSubscription subscription)
        {
            _subscriptions.Remove(subscription.Identifier);
        }

        protected override OutgoingMessage ProcessMessage(IncomingMessage requestMessage, ActionHeader actionHeader)
        {
            switch (actionHeader.Action)
            {
                case Enumeration.Constants.PullAction:
                    return Pull(requestMessage);
                default:
                    return null;
            }
        }

        private OutgoingMessage Pull(IncomingMessage requestMessage)
        {
            var responseMessage = CreateResponseMessage();
            var request = requestMessage.GetPayload<PullRequest>();
            var subsciption = RetrieveSubscription(request);
            var maxElements = CalculateMaxElements(request);
            var maxTime = CalculateMaxTime(request);
            var items = new EnumerationItemList(PullItems(subsciption.Buffer.FetchNotifications(maxElements, maxTime)));

            ReturnTimedOutFaultIfNoPendingEvents(items);
            SetResponseBody(request, items, responseMessage);
            return responseMessage;
        }

        private static OutgoingMessage CreateResponseMessage()
        {
            return new OutgoingMessage()
                .AddHeader(new ActionHeader(Enumeration.Constants.PullResponseAction), false);
        }

        private static void SetResponseBody(PullRequest request, EnumerationItemList items, OutgoingMessage responseMessage)
        {
            responseMessage.SetBody(new SerializerBodyWriter(
                                        new PullResponse
                                            {
                                                Items = items,
                                                EndOfSequence = null,
                                                EnumerationContext = request.EnumerationContext
                                            }));
        }

        /// <summary>
        /// Implementation of R7.2.13-5
        /// </summary>
        /// <param name="items"></param>
        private static void ReturnTimedOutFaultIfNoPendingEvents(EnumerationItemList items)
        {
            if (items.Items.Count() == 0)
            {
                throw new TimedOutFaultException();
            }
        }

        private PullSubscription RetrieveSubscription(PullRequest request)
        {
            PullSubscription subsciption;
            if (!_subscriptions.TryGetValue(request.EnumerationContext.Text, out subsciption))
            {
                throw new InvalidEnumerationContextFaultException();
            }
            return subsciption;
        }

        private static int CalculateMaxElements(PullRequest request)
        {
            return request.MaxElements != null
                       ? request.MaxElements.Value
                       : 1;
        }

        private static TimeSpan CalculateMaxTime(PullRequest request)
        {
            return request.MaxTime != null
                       ? request.MaxTime.Value
                       : TimeSpan.FromSeconds(5);
        }

        private static IEnumerable<EnumerationItem> PullItems(IEnumerable<object> enumerable)
        {
            return enumerable.Select(x => new EnumerationItem(new EndpointReference("http://tempuri.org"), x));
        }
    }
}