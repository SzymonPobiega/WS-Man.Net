using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing
{
   [FilterMapExtensionServiceBehavior]
   public class EventingServer : 
      IWSEventingContract, 
      IWSEventingPullDeliveryContract,
      IFilterMapProvider
   {      
      public void BindWithPullDelivery(string dialect, Type filterType, IEventingRequestHandler eventSource)
      {
         PullDeliverySubscriptionManager enumHandler = new PullDeliverySubscriptionManager(_pullDeliveryServer, eventSource);
         _filterMap.Bind(dialect, filterType);
         _enumHandlers[dialect] = enumHandler;
      }

      public SubscribeResponse Subscribe(SubscribeRequest request)
      {
         Subsciption subsciption = GetManager(request.Filter.Dialect).Subscribe(request.Filter);
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
                         ? new EnumerationContextKey(identifierHeader.Value) 
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

      public PullResponse Pull(PullRequest request)
      {
         return _pullDeliveryServer.Pull(request);
      }

      private ISubscriptionManager GetManager(string filterDialect)
      {
         //TODO: Add fault
         return _enumHandlers[filterDialect];
      }

      public FilterMap ProvideFilterMap()
      {
         return _filterMap;
      }

      private readonly FilterMap _filterMap = new FilterMap();
      private readonly Dictionary<string, Subsciption> _activeSubscriptions = new Dictionary<string, Subsciption>();
      private readonly Dictionary<string, ISubscriptionManager> _enumHandlers = new Dictionary<string, ISubscriptionManager>();      
      private readonly EventingPullDeliveryServer _pullDeliveryServer = new EventingPullDeliveryServer();      
   }
}