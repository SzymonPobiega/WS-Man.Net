using System.Xml;
using System.Xml.Linq;

namespace WSMan.NET
{
    public static class XmlReaderExtensions
    {
        public static XmlReader ReadStartElement(this XmlReader reader, XName elementName)
        {
            reader.ReadStartElement(elementName.LocalName, elementName.Namespace.NamespaceName);
            return reader;
        }

        public static XName Name(this XmlReader reader)
        {
            return XName.Get(reader.LocalName, reader.NamespaceURI);
        }
    }
}