using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing.Server
{
    public static class PullDeliveryExtensions
    {
        public static void EnablePullDeliveryUsing(this EventingServer eventingServer, PullServer pullServer)
        {
            eventingServer.Subscribed += (s, args) => pullServer.RegisterPullHandler(args.Identifier, new EventingPullHandler(args.EventSource));
            eventingServer.Unsubscribed += (s, args) => pullServer.UnregisterPullHandler(args.Identifier);
        }
    }
}