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
            new ClientContext<IWSEnumerationContract>(_endpointUri, _binding.MessageVersion.Addressing,  _proxyFactory, mx => mx.Add(new SelectorSetHeader(selectors))))
         {
            FilterMapExtension.Activate(_filterMap);
            EnumerationModeExtension.Activate(EnumerationMode.EnumerateEPR, null);
            response = ctx.Channel.Enumerate(new EnumerateRequest
                                     {
                                        EnumerationMode = EnumerationMode.EnumerateEPR,
                                        OptimizeEnumeration = _optimize ? new OptimizeEnumeration() : null,
                                        Filter = filter,
                                        MaxElements = new MaxElements(maxElements)
                                     });            
         }
         if (response.Items != null)
         {
            foreach (EnumerationItem item in response.Items.Items)
            {
               yield return item.EprValue;
            }
         }
         EnumerationContextKey context = response.EnumerationContext;
         bool endOfSequence = response.EndOfSequence != null;
         while (!endOfSequence)
         {
            PullResponse pullResponse = PullNextEPRBatch(context, maxElements, selectors);            
            foreach (EnumerationItem item in pullResponse.Items.Items)
            {
               yield return item.EprValue;
            }
            endOfSequence = pullResponse.EndOfSequence != null;
            context = pullResponse.EnumerationContext;
         }
      }

      private PullResponse PullNextEPRBatch(EnumerationContextKey context, int maxElements, IEnumerable<Selector> selectors)
      {
         using (ClientContext<IWSEnumerationContract> ctx =
            new ClientContext<IWSEnumerationContract>(_endpointUri, _binding.MessageVersion.Addressing, _proxyFactory, mx => mx.Add(new SelectorSetHeader(selectors))))
         {
            FilterMapExtension.Activate(_filterMap);
            EnumerationModeExtension.Activate(EnumerationMode.EnumerateEPR, null);
            return ctx.Channel.Pull(new PullRequest
            {
               EnumerationContext = context,               
               MaxElements = new MaxElements(maxElements)
            });
         }
      }
      
      public EnumerationClient(bool optimize, Uri endpointUri, Binding binding)
      {
         _endpointUri = endpointUri;
         _optimize = optimize;
         _binding = binding;
         _proxyFactory = new ChannelFactory<IWSEnumerationContract>(binding);
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
      private readonly Uri _endpointUri;
      private readonly bool _optimize;
      private readonly IChannelFactory<IWSEnumerationContract> _proxyFactory;
      private readonly Binding _binding;
      private readonly FilterMap _filterMap = new FilterMap();
   }
}