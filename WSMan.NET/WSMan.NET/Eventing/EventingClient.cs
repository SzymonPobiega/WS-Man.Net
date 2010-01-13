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

      /// <summary>
      /// Subscribes to the event stream using PULL delivery mode. Events will be processed
      /// by provided callback using a ThreadPool thread.
      /// </summary>
      /// <typeparam name="T">Type to which event messages should be deserialized.</typeparam>
      /// <param name="callback">Callback method used to process events.</param>
      /// <param name="synchronizeCallbackThread">If true, when closing subscription current thread will block until last PULL request ends.</param>
      /// <param name="resourceUri">URI of resource representing event source.</param>
      /// <param name="filter">Optional object used to filter event stream on the server.</param>
      /// <param name="selectors">Optional collection of selector objects which provide
      /// additional description of requested event stream.</param>
      /// <returns></returns>
      public IDisposable SubscribeUsingPullDelivery<T>(Action<T> callback, bool synchronizeCallbackThread, Uri resourceUri, Filter filter, params Selector[] selectors)
      {
         if (callback == null)
         {
            throw new ArgumentNullException("callback", "Callback method must be specified.");
         }
         if (resourceUri == null)
         {
            throw new ArgumentNullException("resourceUri", "URI of resource representing event source must be specified.");
         }
         IPullSubscriptionClient<T> impl = SubscribeUsingPullDelivery<T>(resourceUri, filter, (IEnumerable<Selector>)selectors);
         return new CallbackThreadPoolPullSubscriptionClient<T>(callback, impl, synchronizeCallbackThread);
      }

      /// <summary>
      /// Subscribes to the event stream using PULL delivery mode. Events will be processed
      /// by provided callback using a ThreadPool thread.
      /// </summary>
      /// <typeparam name="T">Type to which event messages should be deserialized.</typeparam>
      /// <param name="callback">Callback method used to process events.</param>
      /// <param name="resourceUri">URI of resource representing event source.</param>
      /// <param name="filter">Optional object used to filter event stream on the server.</param>
      /// <param name="selectors">Optional collection of selector objects which provide
      /// additional description of requested event stream.</param>
      /// <returns></returns>
      public IDisposable SubscribeUsingPullDelivery<T>(Action<T> callback, Uri resourceUri, Filter filter, params Selector[] selectors)
      {
         return SubscribeUsingPullDelivery(callback, true, resourceUri, filter, selectors);
      }

      public IPullSubscriptionClient<T> SubscribeUsingPullDelivery<T>(Uri resourceUri, Filter filter, params Selector[] selectors)
      {
         return SubscribeUsingPullDelivery<T>(resourceUri, filter, (IEnumerable<Selector>)selectors);
      }

      public IPullSubscriptionClient<T> SubscribeUsingPullDelivery<T>(Uri resourceUri, Filter filter, IEnumerable<Selector> selectors)
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
         return new PullSubscriptionClientImpl<T>(_endpointUri, _binding, _filterMap, response.EnumerationContext, resourceUri.ToString(), _proxyFactory);
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