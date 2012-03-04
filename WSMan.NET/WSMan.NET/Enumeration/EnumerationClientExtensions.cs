using System;
using System.Collections.Generic;
using WSMan.NET.Addressing;
using WSMan.NET.Server;
using WSMan.NET.SOAP;

namespace WSMan.NET.Enumeration
{
    public static class EnumerationClientExtensions
    {
        public static int EstimateEnumerationCount(this IMessageBuilder messageBuilder, Filter filter)
        {
            var responseMessage = messageBuilder
                .WithAction(Constants.EnumerateAction)
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

        public static EnumerateResponse StartEnumeration(this IMessageBuilder messageBuilder, Filter filter, EnumerationMode enumerationMode, bool optimize, out IMessageBuilder nextMessageBuilder)
        {
            var responseMessage = messageBuilder
                .WithAction(Constants.EnumerateAction)               
                .AddBody(new EnumerateRequest
                             {
                                 EnumerationMode = enumerationMode,
                                 OptimizeEnumeration = optimize ? new OptimizeEnumeration() : null,
                                 Filter = filter,
                             })
                .SendAndGetResponse(out nextMessageBuilder);

            return responseMessage.GetPayload<EnumerateResponse>();
        }

        public static PullResponse PullNextBatch(this IMessageBuilder messageBuilder, EnumerationContextKey context, int maxElements, out IMessageBuilder nextMessageBuilder)
        {
            var responseMessage = messageBuilder
                .WithAction(Constants.PullAction)               
                .AddBody(new PullRequest
                             {
                                 EnumerationContext = context,
                                 MaxElements = new MaxElements(maxElements)
                             })
                .SendAndGetResponse(out nextMessageBuilder);

            return responseMessage.GetPayload<PullResponse>();
        }

        public static IEnumerable<EndpointReference> EnumerateEPR(this IMessageBuilder messageBuilder, Filter filter, int maxElements, bool optimize)
        {
            IMessageBuilder nextMessageBuilder;
            var response = messageBuilder
                .StartEnumeration(filter, EnumerationMode.EnumerateEPR, optimize, out nextMessageBuilder);

            if (response.Items != null)
            {
                foreach (var item in response.Items)
                {
                    yield return item.EPRValue;
                }
            }
            var context = response.EnumerationContext;
            var endOfSequence = response.EndOfSequence != null;
            while (!endOfSequence)
            {
                var pullResponse = nextMessageBuilder
                    .PullNextBatch(context, maxElements, out nextMessageBuilder);

                foreach (var item in pullResponse.Items)
                {
                    yield return item.EPRValue;
                }
                endOfSequence = pullResponse.EndOfSequence != null;
                context = pullResponse.EnumerationContext;
            }
        }
    }
}