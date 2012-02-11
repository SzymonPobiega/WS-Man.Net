using System.Xml.Linq;

namespace WSMan.NET.Eventing
{
    public class Constants
    {
        public const string NamespaceName = @"http://schemas.xmlsoap.org/ws/2004/08/eventing";
        public static readonly XNamespace Namespace = NamespaceName;

        public const string SubscribeAction = @"http://schemas.xmlsoap.org/ws/2004/08/eventing/Subscribe";
        public const string SubscribeResponseAction = @"http://schemas.xmlsoap.org/ws/2004/08/eventing/SubscribeResponse";

        public const string RenewAction = @"http://schemas.xmlsoap.org/ws/2004/08/eventing/Renew";
        public const string RenewResponseAction = @"http://schemas.xmlsoap.org/ws/2004/08/eventing/RenewResponse";

        public const string UnsubscribeAction = @"http://schemas.xmlsoap.org/ws/2004/08/eventing/Unsubscribe";
        public const string UnsubscribeResponseAction = @"http://schemas.xmlsoap.org/ws/2004/08/eventing/UnsubscribeResponse";
    }
}