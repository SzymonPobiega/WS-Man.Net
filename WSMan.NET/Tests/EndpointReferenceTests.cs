using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using NUnit.Framework;
using WSMan.NET.Addressing;
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
        public void It_writes_address_property()
        {
            var endpointReference = new EndpointReference("http://example.com");

            var xml = ToXml(endpointReference);

            var addressElement = xml.Element(Constants.Namespace + "Address");
            Assert.AreEqual("http://example.com", addressElement.Value);
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
                endpointAddress.WriteXml(writer);
                writer.Flush();
            }

            return XElement.Parse(buffer.ToString());
        }
    }
}
// ReSharper restore InconsistentNaming
