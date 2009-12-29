using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;

namespace WSMan.NET.Enumeration
{
   [FilterMapExtensionServiceBehavior]
   public class EnumerationServer : IWSEnumerationContract, IFilterMapProvider
   {     
      public EnumerateResponse Enumerate(EnumerateRequest request)
      {
         EnumerationContext context = EnumerationContext.Unique();
         if (request.OptimizeEnumeration != null)
         {
            int maxElements = request.MaxElements != null 
               ? request.MaxElements.Value 
               : 1;

            if (request.EnumerationMode == EnumerationMode.EnumerateEPR)
            {
               IEnumerator<object> enumerator = GetHandler(request.Filter.Dialect).Enumerate(context.Text, request.Filter).GetEnumerator();

               bool endOfSequence;
               List<EndpointAddress10> items = PullItems(maxElements, enumerator, out endOfSequence);
               if (!endOfSequence)
               {
                  _activeEnumerations[context] = new EnumerationContextHolder(enumerator);
               }
               return new EnumerateResponse
                         {
                            EnumerateEPRItems = items,
                            EndOfSequence = endOfSequence ? new EndOfSequence() : null,
                            EnumerationContext = endOfSequence ? null : context
                         };
            }
         }
         _activeEnumerations[context] = new EnumerationContextHolder(GetHandler(request.Filter.Dialect).Enumerate(context.Text, request.Filter).GetEnumerator());
         return new EnumerateResponse
                   {
                      EnumerationContext = context,
                      Expires = request.Expires
                   };
      }

      public PullResponse Pull(PullRequest request)
      {
         EnumerationContextHolder holder;
         if (!_activeEnumerations.TryGetValue(request.EnumerationContext, out holder))
         {
            return new PullResponse
            {
               EndOfSequence = new EndOfSequence()
            };
         }

         int maxElements = request.MaxElements != null
                              ? request.MaxElements.Value
                              : 1;

         bool endOfSequence;
         List<EndpointAddress10> items = PullItems(maxElements, holder.Enumerator, out endOfSequence);
         if (endOfSequence)
         {
            _activeEnumerations.Remove(request.EnumerationContext);
         }
         return new PullResponse
                   {
                      EnumerateEPRItems = items,
                      EndOfSequence = endOfSequence ? new EndOfSequence() : null,
                      EnumerationContext = endOfSequence ? null : request.EnumerationContext
                   };
      }

      private static List<EndpointAddress10> PullItems(int maximum, IEnumerator<object> enumerator, out bool endOfSequence)
      {
         int i = 0;         
         List<EndpointAddress10> result = new List<EndpointAddress10>();
         bool moveNext = false;
         while (i < maximum && (moveNext = enumerator.MoveNext()))
         {
            result.Add(EndpointAddress10.FromEndpointAddress((EndpointAddress)enumerator.Current));
            i++;
         }
         endOfSequence = !moveNext || i < maximum;
         return result;
      }

      private IEnumerationRequestHandler GetHandler(string filterDialect)
      {
         //TODO: Add fault if not found
         return _handlerMap[filterDialect];
      }

      public EnumerationServer Bind(string dialect, Type filterType, IEnumerationRequestHandler handler)
      {         
         _filterMap.Bind(dialect, filterType);
         _handlerMap[dialect] = handler;
         return this;
      }

      public FilterMap ProvideFilterMap()
      {
         return _filterMap;
      }      

      private readonly Dictionary<EnumerationContext, EnumerationContextHolder> _activeEnumerations = new Dictionary<EnumerationContext, EnumerationContextHolder>();
      private readonly Dictionary<string, IEnumerationRequestHandler> _handlerMap = new Dictionary<string, IEnumerationRequestHandler>();
      private readonly FilterMap _filterMap = new FilterMap();
   }
}