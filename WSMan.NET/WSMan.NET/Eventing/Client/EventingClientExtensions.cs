using System;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration;
using WSMan.NET.Management.Faults;
using WSMan.NET.Server;
using WSMan.NET.SOAP;

namespace WSMan.NET.Eventing.Client
{
    public static class EventingClientExtensions
    {
        public static string SubscribeUsingPullDelivery(this IMessageBuilder messageBuilder, Filter filter)
        {
            var responseMessage = messageBuilder
                .WithAction(Constants.SubscribeAction)
                .AddBody(new SubscribeRequest
                             {
                                 Delivery = Delivery.Pull(),
                                 Filter = filter
                             })
                .SendAndGetResponse();

            var response = responseMessage.GetPayload<SubscribeResponse>();
            return response.EnumerationContext.Text;
        }

        public static PullResponse PullNextBatch(this IMessageBuilder messageBuilder, string context, int maxElements)
        {
            var requestMessage = messageBuilder
                .WithAction(Enumeration.Constants.PullAction)
                .AddBody(new PullRequest
                             {
                                 MaxTime = new MaxTime(TimeSpan.FromSeconds(1)),
                                 EnumerationContext = new EnumerationContextKey(context),
                                 MaxElements = new MaxElements(maxElements)
                             });
            try
            {
                var responseMessage = requestMessage.SendAndGetResponse();
                return responseMessage.GetPayload<PullResponse>();
            }
            catch (FaultException ex)
            {
                if (new TimedOutFaultException().Equals(ex))
                {
                    return new PullResponse
                               {
                                   EnumerationContext = new EnumerationContextKey(context)
                               };
                }
                throw;
            }
        }

        public static void Unsubscribe(this IMessageBuilder messageBuilder, string context)
        {
            messageBuilder
                .WithAction(Constants.UnsubscribeAction)
                .AddHeader(new IdentifierHeader(context), true)
                .SendAndGetResponse();
        }
    }
}