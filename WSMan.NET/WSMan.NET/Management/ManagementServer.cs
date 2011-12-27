using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WSMan.NET.Enumeration;
using WSMan.NET.Eventing;
using WSMan.NET.Transfer;
using RenewRequest=WSMan.NET.Eventing.RenewRequest;
using RenewResponse=WSMan.NET.Eventing.RenewResponse;

namespace WSMan.NET.Management
{
   public delegate PullResponse PullDelegate(PullRequest request);

   [FilterMapExtensionServiceBehavior]
   [AddressingVersionExtensionServiceBehavior]
   public class ManagementServer :
      IWSTransferContract,
      IWSEnumerationContract,
      IWSEventingContract,
      IFilterMapProvider
   {
      Message IWSTransferContract.Get(Message getRequest)
      {
         return _transferServer.Get(getRequest);
      }

      Message IWSTransferContract.Put(Message putRequest)
      {
         return _transferServer.Put(putRequest);
      }

      Message IWSTransferContract.Create(Message createRequest)
      {
         return _transferServer.Create(createRequest);
      }

      Message IWSTransferContract.Delete(Message deleteRequest)
      {
         return _transferServer.Delete(deleteRequest);
      }

      EnumerateResponse IWSEnumerationContract.Enumerate(EnumerateRequest request)
      {
         return _enumerationServer.Enumerate(request);
      }

      PullResponse IWSEnumerationContract.Pull(PullRequest request)
      {
         ResourceUriHeader resourceUriHeader = OperationContextProxy.Current.FindHeader<ResourceUriHeader>();
           
         //TODO: Fault
         PullDelegate handler = _pullRoutingTable[resourceUriHeader.ResourceUri];
         return handler(request);
      }

      SubscribeResponse IWSEventingContract.Subscribe(SubscribeRequest request)
      {
         return _eventingServer.Subscribe(request);
      }

      void IWSEventingContract.Unsubscribe(UnsubscribeRequest request)
      {
         _eventingServer.Unsubscribe(request);
      }

      public RenewResponse Renew(RenewRequest request)
      {
         return _eventingServer.Renew(request);
      }

      FilterMap IFilterMapProvider.ProvideFilterMap()
      {
         return _filterMap;
      }

      public ManagementServer()
      {
         _managementHandler = new ManagementTransferRequestHandler();
         _transferServer = new TransferServer(_managementHandler);
         _enumerationServer = new EnumerationServer();
         _pullDeliveryServer = new EventingPullDeliveryServer();
         _eventingServer = new EventingServer(_pullDeliveryServer);         
      }

      public void BindManagement(Uri resourceUri, IManagementRequestHandler managementRequestHandler)
      {
         _managementHandler.Bind(resourceUri, managementRequestHandler);
      }

      public void BindEnumeration(Uri resoureceUri, string dialect, Type filterType, IEnumerationRequestHandler enumerationRequestHandler)
      {
         _filterMap.Bind(dialect, filterType);
         _pullRoutingTable[resoureceUri.ToString()] = _enumerationServer.Pull;
         _enumerationServer.Bind(resoureceUri, dialect, filterType, enumerationRequestHandler);
      }

      public void BindPullEventing(Uri resourceUri, string dialect, Type filterType, IEventingRequestHandler eventingRequestHandler, Uri deliveryResourceUri)
      {
         _filterMap.Bind(dialect, filterType);
         _pullRoutingTable[deliveryResourceUri.ToString()] = _pullDeliveryServer.Pull;
         _eventingServer.BindWithPullDelivery(resourceUri, dialect, filterType, eventingRequestHandler, deliveryResourceUri);
      }      
      
      private readonly Dictionary<string, PullDelegate> _pullRoutingTable = new Dictionary<string, PullDelegate>();
      private readonly TransferServer _transferServer;      
      private readonly EventingServer _eventingServer;
      private readonly EnumerationServer _enumerationServer;
      private readonly EventingPullDeliveryServer _pullDeliveryServer;
      private readonly ManagementTransferRequestHandler _managementHandler;
      private readonly FilterMap _filterMap = new FilterMap();     
   }
}