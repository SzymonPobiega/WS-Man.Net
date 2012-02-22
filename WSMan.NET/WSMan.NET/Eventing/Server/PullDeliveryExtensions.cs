using WSMan.NET.Server;

namespace WSMan.NET.Eventing.Server
{
    public static class PullDeliveryExtensions
    {
        public static IRequestHandler EnablePullDelivery(this EventingServer eventingServer)
        {
            var pullDeliveryServer = new EventingPullDeliveryServer();
            eventingServer.Subscribed += pullDeliveryServer.OnSubscriptionAdded;
            eventingServer.Unsubscribed += pullDeliveryServer.OnSubscriptionRemoved;
            return pullDeliveryServer;
        }
    }
}