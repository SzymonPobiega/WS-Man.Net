using WSMan.NET.Server;

namespace WSMan.NET.Addressing
{
    public static class MessageBuilderExtensions
    {
        public static IMessageBuilder WithAction(this IMessageBuilder messageBuilder, string action)
        {
            return messageBuilder.AddHeader(new ActionHeader(action), true);
        }
    }
}