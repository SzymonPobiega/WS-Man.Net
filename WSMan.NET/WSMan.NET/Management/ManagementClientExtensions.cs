using System.Collections.Generic;
using WSMan.NET.Addressing;
using WSMan.NET.Server;
using WSMan.NET.Transfer;

namespace WSMan.NET.Management
{
    public static class ManagementClientExtensions
    {
        public static T Get<T>(this IMessageBuilder messageBuilder, string resourceUri, string fragmentTransferExpression, IEnumerable<Selector> selectors)
        {
            return messageBuilder
                .WithResourceUri(resourceUri)
                .WithSelectors(selectors)
                .AddHeader(new FragmentTransferHeader(fragmentTransferExpression), true)
                .Get<T>();
        }

        public static T Put<T>(this IMessageBuilder messageBuilder, string resourceUri, string fragmentTransferExpression, object payload, IEnumerable<Selector> selectors)
        {
            return messageBuilder
                .WithResourceUri(resourceUri)
                .WithSelectors(selectors)
                .AddHeader(new FragmentTransferHeader(fragmentTransferExpression), true)
                .Put<T>(payload);
        }

        public static EndpointReference Create(this IMessageBuilder messageBuilder, string resourceUri, object payload)
        {
            return messageBuilder
                .WithResourceUri(resourceUri)
                .Create(payload);
        }

        public static void Delete(this IMessageBuilder messageBuilder, string resourceUri, IEnumerable<Selector> selectors)
        {
            messageBuilder
                .WithResourceUri(resourceUri)
                .WithSelectors(selectors)
                .Delete();
        }

        public static IMessageBuilder WithResourceUri(this IMessageBuilder messageBuilder, string resourceUri)
        {
            return messageBuilder.AddHeader(new ResourceUriHeader(resourceUri), true);
        }

        public static IMessageBuilder WithSelectors(this IMessageBuilder messageBuilder, IEnumerable<Selector> selectors)
        {
            return messageBuilder.AddHeader(new SelectorSetHeader(selectors), true);
        }
    }
}