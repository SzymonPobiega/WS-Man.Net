using System.Xml.Linq;

namespace WSMan.NET.SOAP
{
    public static class Constants
    {
        public static readonly XNamespace EnvelopeNamespace = "http://www.w3.org/2003/05/soap-envelope";
        public static readonly XName Envelope = EnvelopeNamespace + "Envelope";
        public static readonly XName Header = EnvelopeNamespace + "Header";
        public static readonly XName Body = EnvelopeNamespace + "Body";
    }
}