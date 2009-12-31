using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;

namespace WSMan.NET.Eventing
{
   public class PullSubscriptionClientImpl : IPullSubscriptionClient
   {
      public IEnumerable<object> PullOnce()
      {
         PullResponse pullResponse = PullNextBatch(_context, 100, new Selector[] {});
         _context = pullResponse.EnumerationContext;
         return pullResponse.EnumerateEPRItems.Select(x => x.ToEndpointAddress()).Cast<object>();         
      }      

      public IEnumerable<object> Pull()
      {
         bool endOfSequence = false;
         while (!endOfSequence)
         {
            PullResponse pullResponse = PullNextBatch(_context, 100, new Selector[] { });
            foreach (EndpointAddress10 item in pullResponse.EnumerateEPRItems)
            {
               yield return item.ToEndpointAddress();
            }
            endOfSequence = pullResponse.EndOfSequence != null;
            _context = pullResponse.EnumerationContext;
         }
      }

      private PullResponse PullNextBatch(EnumerationContextKey context, int maxElements, IEnumerable<Selector> selectors)
      {
         using (ClientContext<IWSEnumerationContract> ctx =
            new ClientContext<IWSEnumerationContract>(_endpointUri, _binding.MessageVersion.Addressing, _proxyFactory, mx => mx.Add(new SelectorSetHeader(selectors))))
         {
            FilterMapExtension.Activate(_filterMap);
            return ctx.Channel.Pull(new PullRequest
            {
               MaxTime = new MaxTime(TimeSpan.FromSeconds(1)),
               EnumerationContext = context,
               MaxElements = new MaxElements(maxElements)
            });
         }
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

      public PullSubscriptionClientImpl(Uri endpointUri, Binding binding, FilterMap filterMap, EnumerationContextKey context)
      {
         _endpointUri = endpointUri;
         _context = context;
         _filterMap = filterMap;
         _binding = binding;
         _proxyFactory = new ChannelFactory<IWSEnumerationContract>(binding);
      }

      private bool _disposed;
      private readonly Uri _endpointUri;
      private readonly IChannelFactory<IWSEnumerationContract> _proxyFactory;
      private readonly FilterMap _filterMap = new FilterMap();
      private readonly Binding _binding;
      private EnumerationContextKey _context;
   }
}