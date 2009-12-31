using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;

namespace WSMan.NET.Eventing
{
   [FilterMapExtensionServiceBehavior]
   [AddressingVersionExtensionServiceBehavior]
   public class EventingServer : 
      IWSEventingContract, 
      IWSEventingPullDeliveryContract,
      IFilterMapProvider
   {
      public void BindWithPullDelivery(
         Uri listeningResourceUri, 
         Uri deliveryResourceUri,
         IEventingRequestHandler eventSource, 
         string dialect, 
         Type filterType)
      {
         PullDeliverySubscriptionManager enumHandler = new PullDeliverySubscriptionManager(deliveryResourceUri.ToString(), _pullDeliveryServer, eventSource);
         _filterMap.Bind(dialect, filterType);
         _enumHandlers[listeningResourceUri.ToString()] = enumHandler;
      }

      public SubscribeResponse Subscribe(SubscribeRequest request)
      {
         //Check
         SelectorSetHeader selectorSetHeader =
            SelectorSetHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);

         //Check
         ResourceUriHeader resourceUriHeader =
            ResourceUriHeader.ReadFrom(OperationContext.Current.IncomingMessageHeaders);

         Subsciption subsciption = GetManager(resourceUriHeader.ResourceUri).Subscribe(
            request.Filter,
            selectorSetHeader != null ? selectorSetHeader.Selectors : (IEnumerable<Selector>)new Selector[] {});
         
         lock (_activeSubscriptions)
         {            
            _activeSubscriptions[subsciption.Identifier] = subsciption;            
         }
         
         return new SubscribeResponse
                   {
                      //R7.2.4-1
                      SubscriptionManager = new SubscriptionManager(subsciption.Identifier, OperationContext.Current.IncomingMessageHeaders.To, subsciption.DeliveryResourceUri),                      
                      EnumerationContext = request.Delivery.Mode == Const.DeliveryModePull 
                         ? new EnumerationContextKey(subsciption.Identifier) 
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

      private ISubscriptionManager GetManager(string resourceUri)
      {
         //TODO: Add fault
         return _enumHandlers[resourceUri];
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