using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;

namespace WSMan.NET.Eventing
{
   public class EventingClient : IDisposable
   {
      public void BindFilterDialect(string dialect, Type implementationType)
      {
         _filterMap.Bind(dialect, implementationType);
      }

      public IPullSubscriptionClient<T> SubscribeWithPullDelivery<T>(Uri resourceUri, Filter filter, params Selector[] selectors)
      {
         return SubscribeWithPullDelivery<T>(resourceUri, filter, (IEnumerable<Selector>)selectors);
      }

      public IPullSubscriptionClient<T> SubscribeWithPullDelivery<T>(Uri resourceUri, Filter filter, IEnumerable<Selector> selectors)
      {
         SubscribeResponse response;
         using (ClientContext<IWSEventingContract> ctx =
            new ClientContext<IWSEventingContract>(_endpointUri, _binding.MessageVersion.Addressing, _proxyFactory, mx => {
               mx.Add(new SelectorSetHeader(selectors)); 
               mx.Add(new ResourceUriHeader(resourceUri.ToString()));
            }))
         {
            FilterMapExtension.Activate(_filterMap);
            response = ctx.Channel.Subscribe(new SubscribeRequest
                                                {
                                                   Delivery = Delivery.Pull(),                                                                                                      
                                                   Filter = filter                                                   
                                                });            
         }
         return new PullSubscriptionClientImpl<T>(_endpointUri, _binding, _filterMap, response.EnumerationContext);
      }      
      
      public EventingClient(Uri endpointUri, Binding binding)
      {
         _endpointUri = endpointUri;
         _binding = binding;
         _proxyFactory = new ChannelFactory<IWSEventingContract>(binding);
      }

      public void Dispose()
      {
         if (_disposed)
         {
            return;
         }
         try
         {
            _proxyFactory.Close();
         }
         catch (Exception)
         {
            _proxyFactory.Abort();
         }
         _disposed = true;
      }

      private bool _disposed;
      private readonly Binding _binding;
      private readonly Uri _endpointUri;
      private readonly IChannelFactory<IWSEventingContract> _proxyFactory;
      private readonly FilterMap _filterMap = new FilterMap();

   }
}