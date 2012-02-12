using System;
using System.Collections.Generic;
using WSMan.NET.Addressing;
using WSMan.NET.Management;
using WSMan.NET.Server;
using WSMan.NET.SOAP;

namespace WSMan.NET.Enumeration
{
    public static class EnumerationClientExtensions
    {
        public static int EstimateEnumerationCount(this IMessageBuilder messageBuilder, string resourceUri, Filter filter, IEnumerable<Selector> selectors)
        {
            var responseMessage = messageBuilder
                .WithAction(Constants.EnumerateAction)
                .WithResourceUri(resourceUri)
                .WithSelectors(selectors)
                .AddHeader(new RequestTotalItemsCountEstimateHeader(), true)
                .AddBody(new EnumerateRequest
                             {
                                 EnumerationMode = EnumerationMode.EnumerateEPR,
                                 Filter = filter
                             })
                .SendAndGetResponse();

            var totalCountEstimateHeader = responseMessage.GetHeader<TotalItemsCountEstimateHeader>();
            if (totalCountEstimateHeader == null)
            {
                throw new InvalidOperationException("Total items count header is missing from the response.");
            }
            return totalCountEstimateHeader.Value;
        }

        public static  EnumerateResponse StartEnumeration(this IMessageBuilder messageBuilder, string resourceUri, IEnumerable<Selector> selectors, Filter filter, EnumerationMode enumerationMode, bool optimize)
        {
            var responseMessage = messageBuilder
                .WithAction(Constants.EnumerateAction)
                .WithResourceUri(resourceUri)
                .WithSelectors(selectors)
                .AddBody(new EnumerateRequest
                             {
                                 EnumerationMode = enumerationMode,
                                 OptimizeEnumeration = optimize ? new OptimizeEnumeration() : null,
                                 Filter = filter,
                             })
                .SendAndGetResponse();

            return responseMessage.GetPayload<EnumerateResponse>();
        }

        public static PullResponse PullNextBatch(this IMessageBuilder messageBuilder, EnumerationContextKey context, string resourceUri, int maxElements, IEnumerable<Selector> selectors)
        {
            var responseMessage = messageBuilder
                .WithAction(Constants.PullAction)
                .WithResourceUri(resourceUri)
                .WithSelectors(selectors)
                .AddBody(new PullRequest
                             {
                                 EnumerationContext = context,
                                 MaxElements = new MaxElements(maxElements)
                             })
                .SendAndGetResponse();

            return responseMessage.GetPayload<PullResponse>();
        }
    }
}