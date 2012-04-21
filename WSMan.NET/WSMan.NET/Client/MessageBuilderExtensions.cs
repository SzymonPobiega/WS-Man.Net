using System.Collections.Generic;

namespace WSMan.NET.Client
{
    public static class MessageBuilderExtensions
    {        
        public static IMessageBuilder AddHeader(this IMessageBuilder messageBuilder, IMessageHeaderWithMustUnderstandSpecification header)
        {
            return messageBuilder.AddHeader(header.Header, header.MustUnderstand);
        }

        public static IMessageBuilder AddHeaders(this IMessageBuilder messageBuilder, IEnumerable<IMessageHeaderWithMustUnderstandSpecification> headers)
        {
            foreach (var header in headers)
            {
                messageBuilder.AddHeader(header.Header, header.MustUnderstand);
            }
            return messageBuilder;
        }
    }
}