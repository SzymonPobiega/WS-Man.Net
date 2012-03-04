using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using NUnit.Framework;
using WSMan.NET.Addressing;
using WSMan.NET.SOAP;
using WSMan.NET.Transfer;
using Constants = WSMan.NET.Addressing.Constants;

// ReSharper disable InconsistentNaming
namespace WSMan.NET
{
    [TestFixture]
    public class EndpointReferenceTests
    {
        [Test]
        public void It_reads_address_property()
        {
            var endpointReferenceElement
                = new XElement(Constants.Namespace + "EndpointReference",
                               new XElement(Constants.Namespace + "Address",
                                            new XText("http://example.com")));

            var endpointReference = FromXml(endpointReferenceElement);

            Assert.AreEqual("http://example.com",endpointReference.Address);
        }

        [Test]
        public void It_reads_embedded_headers()
        {
            var endpointReferenceElement
                = new XElement(Constants.Namespace + "EndpointReference",
                               new XElement(Constants.Namespace + "Address",
                                            new XText("http://example.com")),
                               new XElement(Constants.Namespace + "ReferenceProperties",
                                            new XElement("someHeader", new XText("someValue"))));

            var endpointReference = FromXml(endpointReferenceElement);

            var header = endpointReference.GetProperty("someHeader");

            Assert.AreEqual("someValue", ((XText)header.Content.First()).Value);
        }
        
        [Test]
        public void It_writes_address_property()
        {
            var endpointReference = new EndpointReference("http://example.com");

            var xml = ToXml(endpointReference);

            var addressElement = xml.Element(Constants.Namespace + "Address");
            Assert.AreEqual("http://example.com", addressElement.Value);
        }

        [Test]
        public void It_writes_embedded_headers()
        {
            var endpointReference = new EndpointReference("http://example.com");
            endpointReference.AddProperty(new MessageHeader("someHeader", new[] {new XText("someText")}, false));

            var xml = ToXml(endpointReference);

            var parametersElement = xml.Element(Constants.Namespace + "ReferenceProperties");
            var headerElement = parametersElement.Element("someHeader");
            var headerText = (XText)headerElement.FirstNode;
            Assert.AreEqual("someText", headerText.Value);
        }

        private static EndpointReference FromXml(XElement endpointReferenceElement)
        {
            var endpointAddress = new EndpointReference();
            using (var reader = endpointReferenceElement.ToReader())
            {
                endpointAddress.ReadXml(reader);
            }
            return endpointAddress;
        }


        private static XElement ToXml(EndpointReference endpointAddress)
        {
            var buffer = new StringBuilder();
            using (var writer = XmlWriter.Create(buffer))
            {
                endpointAddress.WriteOuterXml(writer);
                writer.Flush();
            }

            return XElement.Parse(buffer.ToString());
        }
    }
}
// ReSharper restore InconsistentNaming
