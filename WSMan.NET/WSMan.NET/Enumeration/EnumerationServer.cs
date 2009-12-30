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
         EnumerationContextKey contextKey = EnumerationContextKey.Unique();         
         EnumerationContext context = new EnumerationContext(contextKey.Text, request.Filter);
         if (request.OptimizeEnumeration != null)
         {
            return HandleOptimizedEnumerate(contextKey, request, context);
         }

         IEnumerator<object> enumerator = GetHandler(request.Filter.Dialect).Enumerate(context).GetEnumerator();
         _activeEnumerations[contextKey] = new EnumerationState(enumerator);
         return new EnumerateResponse
                   {
                      EnumerationContext = contextKey,
                      Expires = request.Expires
                   };
      }

      private EnumerateResponse HandleOptimizedEnumerate(EnumerationContextKey contextKey, EnumerateRequest request, EnumerationContext context)
      {
         int maxElements = request.MaxElements != null 
                              ? request.MaxElements.Value 
                              : 1;

         if (request.EnumerationMode == EnumerationMode.EnumerateEPR)
         {
            IEnumerator<object> enumerator = GetHandler(request.Filter.Dialect).Enumerate(context).GetEnumerator();

            bool endOfSequence;
            List<EndpointAddress10> items = PullItems(maxElements, enumerator, out endOfSequence);
            if (!endOfSequence)
            {
               _activeEnumerations[contextKey] = new EnumerationState(enumerator);
            }
            return new EnumerateResponse
                      {
                         EnumerateEPRItems = items,
                         EndOfSequence = endOfSequence ? new EndOfSequence() : null,
                         EnumerationContext = endOfSequence ? null : contextKey
                      };
         }
         throw new NotSupportedException();
      }

      public PullResponse Pull(PullRequest request)
      {
         EnumerationState holder;
         if (!_activeEnumerations.TryGetValue(request.EnumerationContext, out holder))
         {
            return new PullResponse
            {
               //TODO: Return fault
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

      private readonly Dictionary<EnumerationContextKey, EnumerationState> _activeEnumerations = new Dictionary<EnumerationContextKey, EnumerationState>();
      private readonly Dictionary<string, IEnumerationRequestHandler> _handlerMap = new Dictionary<string, IEnumerationRequestHandler>();
      private readonly FilterMap _filterMap = new FilterMap();
   }
}