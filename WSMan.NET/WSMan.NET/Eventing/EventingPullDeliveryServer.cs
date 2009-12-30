using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing
{   
   public class EventingPullDeliveryServer : IWSEventingPullDeliveryContract
   {      
      public PullResponse Pull(PullRequest request)
      {
         PullSubscription subsciption;
         if (!_subscriptions.TryGetValue(request.EnumerationContext.Text, out subsciption))
         {
            return new PullResponse
                      {
                         //TODO: Return fault
                         EndOfSequence = new EndOfSequence()
                      };
         }

         int maxElements = request.MaxElements != null
                              ? request.MaxElements.Value
                              : 1;

         TimeSpan maxTime = request.MaxTime != null 
            ? request.MaxTime.Value 
            : TimeSpan.FromSeconds(5);
         
         List<EndpointAddress10> items = PullItems(subsciption.Buffer.FetchNotifications(maxElements, maxTime));

         return new PullResponse
                   {
                      EnumerateEPRItems = items,
                      EndOfSequence = null,
                      EnumerationContext = request.EnumerationContext
                   };
      }

      private static List<EndpointAddress10> PullItems(IEnumerable<object> enumerable)
      {
         return enumerable.Cast<EndpointAddress>()
            .Select(x => EndpointAddress10.FromEndpointAddress(x))
            .ToList();
      }

      public void AddSubscription(PullSubscription subscription)
      {
         _subscriptions[subscription.Identifier] = subscription;
      }

      public void RemoveSubscription(PullSubscription subscription)
      {
         _subscriptions.Remove(subscription.Identifier);
      }

      private readonly Dictionary<string, PullSubscription> _subscriptions = new Dictionary<string, PullSubscription>();
   }
}