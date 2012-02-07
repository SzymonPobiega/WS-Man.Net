using System.Xml.Linq;

namespace WSMan.NET.Enumeration
{
    public class Constants
    {
        public const string NamespaceName = @"http://schemas.xmlsoap.org/ws/2004/09/enumeration";
        public static readonly XNamespace Namespace = NamespaceName;

        public const string FaultAction = @"http://schemas.xmlsoap.org/ws/2004/09/enumeration/fault";
        public const string EnumerateAction = @"http://schemas.xmlsoap.org/ws/2004/09/enumeration/Enumerate";
        public const string EnumerateResponseAction = @"http://schemas.xmlsoap.org/ws/2004/09/enumeration/EnumerateResponse";
        public const string PullAction = @"http://schemas.xmlsoap.org/ws/2004/09/enumeration/Pull";
        public const string PullResponseAction = @"http://schemas.xmlsoap.org/ws/2004/09/enumeration/PullResponse";
    }
}