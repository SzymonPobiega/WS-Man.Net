using System.Xml.Linq;

namespace WSMan.NET.SOAP
{
    public static class Constants
    {
        public const string NamespaceName = "http://www.w3.org/2003/05/soap-envelope";
        public static readonly XNamespace Namespace = NamespaceName;
        public static readonly XName Envelope = Namespace + "Envelope";
        public static readonly XName Header = Namespace + "Header";
        public static readonly XName Body = Namespace + "Body";
    }
}