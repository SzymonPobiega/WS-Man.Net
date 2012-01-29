using System.Xml;
using System.Xml.Linq;

namespace WSMan.NET
{
    public static class XmlWriterExtensions
    {
        public static XmlWriter WriteStartElement(this XmlWriter writer, XName elementName)
        {
            writer.WriteStartElement(elementName.LocalName, elementName.Namespace.NamespaceName);
            return writer;
        }
    }
}