using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WSMan.NET.Management;

namespace WSMan.NET.Enumeration
{
   public class EnumerationClient : IDisposable
   {      
      public void BindFilterDialect(string dialect, Type implementationType)
      {
         _filterMap.Bind(dialect, implementationType);
      }

      public IEnumerable<EndpointAddress> EnumerateEPR(Filter filter, int maxElements, params Selector[] selectors)
      {
         return EnumerateEPR(filter, maxElements, (IEnumerable<Selector>) selectors);
      }

      public IEnumerable<EndpointAddress> EnumerateEPR(Filter filter, int maxElements, IEnumerable<Selector> selectors)
      {
         EnumerateResponse response;
         using (ClientContext<IWSEnumerationContract> ctx = 
            new ClientContext<IWSEnumerationContract>(_endpointUri, _proxyFactory, mx => mx.Add(new SelectorSetHeader(selectors))))
         {
            FilterMapExtension.Activate(_filterMap);
            response = ctx.Channel.Enumerate(new EnumerateRequest
                                     {
                                        EnumerationMode = EnumerationMode.EnumerateEPR,
                                        OptimizeEnumeration = _optimizeEnumeration ? new OptimizeEnumeration() : null,
                                        Filter = filter,
                                        MaxElements = new MaxElements(maxElements)
                                     });            
         }
         if (response.EnumerateEPRItems != null)
         {
            foreach (EndpointAddress10 item in response.EnumerateEPRItems)
            {
               yield return item.ToEndpointAddress();
            }
         }
         EnumerationContext context = response.EnumerationContext;
         bool endOfSequence = response.EndOfSequence != null;
         while (!endOfSequence)
         {
            PullResponse pullResponse = PullNextBatch(context, maxElements, selectors);            
            foreach (EndpointAddress10 item in pullResponse.EnumerateEPRItems)
            {
               yield return item.ToEndpointAddress();
            }
            endOfSequence = pullResponse.EndOfSequence != null;
            context = pullResponse.EnumerationContext;
         }
      }

      private PullResponse PullNextBatch(EnumerationContext context, int maxElements, IEnumerable<Selector> selectors)
      {
         using (ClientContext<IWSEnumerationContract> ctx =
            new ClientContext<IWSEnumerationContract>(_endpointUri, _proxyFactory, mx => mx.Add(new SelectorSetHeader(selectors))))
         {
            FilterMapExtension.Activate(_filterMap);
            return ctx.Channel.Pull(new PullRequest
            {
               EnumerationContext = context,               
               MaxElements = new MaxElements(maxElements)
            });
         }
      }
      
      public EnumerationClient(bool optimizeEnumeration, Uri endpointUri, Binding binding)
      {
         _optimizeEnumeration = optimizeEnumeration;
         _endpointUri = endpointUri;
         _proxyFactory = new ChannelFactory<IWSEnumerationContract>(binding);
      }

      public void Dispose()
      {
         if (!_disposed)
         {
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
      }

      private bool _disposed;
      private readonly bool _optimizeEnumeration;
      private readonly Uri _endpointUri;
      private readonly IChannelFactory<IWSEnumerationContract> _proxyFactory;
      private readonly FilterMap _filterMap = new FilterMap();
   }
}