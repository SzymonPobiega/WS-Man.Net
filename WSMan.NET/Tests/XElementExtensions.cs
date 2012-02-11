using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace WSMan.NET
{
    public static class XElementExtensions
    {
        public static XmlReader ToReader(this XElement element)
        {
            var buffer = new StringBuilder();
            using (var writer = new StringWriter(buffer))
            {
                element.Save(writer);
                writer.Flush();
            }
            return XmlReader.Create(new StringReader(buffer.ToString()), new XmlReaderSettings
                                                                             {
                                                                                 IgnoreWhitespace = true
                                                                             });
        }

        public static XElement ParseGeneratedXml(Action<XmlWriter> writeAction)
        {
            var buffer = new StringBuilder();
            using (var writer = XmlWriter.Create(buffer))
            {
                writeAction(writer);
                writer.Flush();
            }

            return XElement.Parse(buffer.ToString());
        }
    }
}