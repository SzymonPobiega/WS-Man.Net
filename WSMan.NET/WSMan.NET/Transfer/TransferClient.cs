using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WSMan.NET.Transfer
{
   public delegate void HeaderCreatorDelegate(MessageHeaders headers);

   public delegate void AddressHeaderCreatorDelegate(Collection<AddressHeader> addressHeaders);

   public class TransferClient
   {
      private readonly Uri _endpointUri;
      private readonly IChannelFactory<ITransferContract> _proxyFactory;
      private readonly MessageFactory _factory;
      private readonly AddressingVersion _addressingVersion;

      public TransferClient(Uri endpointUri, IChannelFactory<ITransferContract> proxyFactory, MessageVersion version)
      {
         _endpointUri = endpointUri;
         _proxyFactory = proxyFactory;
         _factory = new MessageFactory(version);
         _addressingVersion = version.Addressing;
      }

      public T Get<T>(AddressHeaderCreatorDelegate addressHeaderCreatorDelegate, HeaderCreatorDelegate headerCreatorCallback)
      {
         using (ClientContext<ITransferContract> ctx = new ClientContext<ITransferContract>(_endpointUri, _addressingVersion, _proxyFactory, addressHeaderCreatorDelegate))
         {
            headerCreatorCallback(OperationContext.Current.OutgoingMessageHeaders);
            return (T)_factory.DeserializeMessageWithPayload(ctx.Channel.Get(_factory.CreateGetRequest()), typeof(T));
         }
      }

      public T Put<T>(AddressHeaderCreatorDelegate addressHeaderCreatorDelegate, HeaderCreatorDelegate headerCreatorCallback, object payload)
      {
         using (ClientContext<ITransferContract> ctx = new ClientContext<ITransferContract>(_endpointUri, _addressingVersion, _proxyFactory, addressHeaderCreatorDelegate))
         {
            headerCreatorCallback(OperationContext.Current.OutgoingMessageHeaders);            
            return (T)_factory.DeserializeMessageWithPayload(ctx.Channel.Put(_factory.CreatePutRequest(payload)), typeof(T));
         }
      }

      public EndpointAddress Create(AddressHeaderCreatorDelegate addressHeaderCreatorDelegate, HeaderCreatorDelegate headerCreatorCallback, object payload)
      {
         using (ClientContext<ITransferContract> ctx = new ClientContext<ITransferContract>(_endpointUri, _addressingVersion, _proxyFactory, addressHeaderCreatorDelegate))
         {
            headerCreatorCallback(OperationContext.Current.OutgoingMessageHeaders);
            return _factory.DeserializeCreateResponse(ctx.Channel.Create(_factory.CreateCreateRequest(payload)));
         }
      }

      public void Delete(AddressHeaderCreatorDelegate addressHeaderCreatorDelegate, HeaderCreatorDelegate headerCreatorCallback)
      {
         using (ClientContext<ITransferContract> ctx = new ClientContext<ITransferContract>(_endpointUri, _addressingVersion, _proxyFactory, addressHeaderCreatorDelegate))
         {
            headerCreatorCallback(OperationContext.Current.OutgoingMessageHeaders);
            ctx.Channel.Delete(_factory.CreateDeleteRequest());
         }
      }      
   }
}