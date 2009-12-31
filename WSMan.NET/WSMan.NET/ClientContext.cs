using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WSMan.NET.Transfer;

namespace WSMan.NET
{
   public class ClientContext<T> : IDisposable
   {
      private readonly T _channel;
      private readonly OperationContextScope _scope;

      public ClientContext(Uri endpointUri, AddressingVersion addressingVersion, IChannelFactory<T> proxyFactory, AddressHeaderCreatorDelegate addressHeaderCreatorDelegate)
      {
         EndpointAddressBuilder builder = new EndpointAddressBuilder();
         addressHeaderCreatorDelegate(builder.Headers);
         builder.Uri = endpointUri;

         _channel = proxyFactory.CreateChannel(builder.ToEndpointAddress());
         _scope = new OperationContextScope((IContextChannel)_channel);
         AddressingVersionExtension.Activate(addressingVersion);
      }

      public T Channel
      {
         get { return _channel; }
      }

      public void Dispose()
      {
         _scope.Dispose();

         ICommunicationObject comm = (ICommunicationObject)_channel;
         if (comm != null)
         {
            try
            {
               if (comm.State != CommunicationState.Faulted)
               {
                  comm.Close();
               }
               else
               {
                  comm.Abort();
               }
            }
            catch (CommunicationException)
            {
               comm.Abort();
            }
            catch (TimeoutException)
            {
               comm.Abort();
            }
            catch (Exception)
            {
               comm.Abort();
               throw;
            }
         }
      }
   }
}