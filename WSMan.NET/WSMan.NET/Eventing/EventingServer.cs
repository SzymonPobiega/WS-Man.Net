using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing
{
   public class EventingServer : IWSEventingContract
   {      
      public void BindWithPullDelivery(EnumerationServer enumerationServer, string dialect, Type filterType, IEventingRequestHandler eventSource)
      {         
         PullDeliverySubscriptionManager enumHandler = new PullDeliverySubscriptionManager(eventSource);
         enumerationServer.Bind(dialect, filterType, enumHandler);
         _enumHandlers[dialect] = enumHandler;
      }

      public SubscribeResponse Subscribe(SubscribeRequest request)
      {
         Subsciption subsciption = GetManager(request.Filter.Dialect).Subscribe();
         IdentifierHeader identifierHeader = new IdentifierHeader(subsciption.Identifier);
         
         lock (_activeSubscriptions)
         {            
            _activeSubscriptions[identifierHeader.Value] = subsciption;            
         }

         //R7.2.4-1
         var subscriptionManagerAddress = new EndpointAddressBuilder
                                             {                                                
                                                Uri = OperationContext.Current.IncomingMessageHeaders.To,                                                
                                             };
         subscriptionManagerAddress.Headers.Add(identifierHeader);

         return new SubscribeResponse
                   {
                      SubscriptionManager = new EndpointReference(subscriptionManagerAddress),                      
                      EnumerationContext = request.Delivery.Mode == Const.DeliveryModePull 
                         ? new EnumerationContext(identifierHeader.Value) 
                         : null
                   };
      }

      public void Unsubscribe(UnsubscribeRequest request)
      {
         IdentifierHeader identifierHeader =
            IdentifierHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);
         
         lock (_activeSubscriptions)
         {
            Subsciption toRemove;
            if (_activeSubscriptions.TryGetValue(identifierHeader.Value, out toRemove))
            {
               toRemove.Dispose();
               _activeSubscriptions.Remove(identifierHeader.Value);
            }            
         }         
      }

      private ISubscriptionManager GetManager(string filterDialect)
      {
         //TODO: Add fault
         return _enumHandlers[filterDialect];
      }

      private readonly Dictionary<string, Subsciption> _activeSubscriptions = new Dictionary<string, Subsciption>();
      private readonly Dictionary<string, ISubscriptionManager> _enumHandlers = new Dictionary<string, ISubscriptionManager>();
   }
}