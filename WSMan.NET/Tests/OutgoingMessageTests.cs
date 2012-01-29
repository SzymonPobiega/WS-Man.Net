using System;
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
    public class OutgoingMessageTests
    {
        [Test]
        public void It_writes_headers()
        {
            var message = new OutgoingMessage();

            var headerName = XName.Get("someHeader","someNamespace");
            var header = new MessageHeader(
                headerName, 
                new XNode[] {new XText("someText")}, 
                false);
            message.AddHeader(header);

            var envelope = XElementExtensions.ParseGeneratedXml(message.Write);

            Assert.AreEqual(SOAP.Constants.Envelope, envelope.Name);
            var headerSectionElement = envelope.Element(SOAP.Constants.Header);
            var headerElement = headerSectionElement.Element(headerName);
            var headerContent = (XText)headerElement.FirstNode;
            Assert.AreEqual("someText", headerContent.Value);
        }

        [Test]
        public void It_writes_body()
        {
            var bodyWriter = new XElementWriter(new XElement("Body"));
            var message = new OutgoingMessage();
            message.SetBody(bodyWriter);

            var envelope = XElementExtensions.ParseGeneratedXml(message.Write);

            Assert.AreEqual(SOAP.Constants.Envelope, envelope.Name);
            var bodySectionElement = envelope.Element(SOAP.Constants.Body);
            var bodyElement = (XElement) bodySectionElement.FirstNode;
            Assert.AreEqual("Body", bodyElement.Name.LocalName);
        }

        private class NullWriter : IBodyWriter
        {
            public void OnWriteBodyContents(XmlWriter writer)
            {
            }
        }

        private class XElementWriter : IBodyWriter
        {
            private readonly XElement _body;

            public XElementWriter(XElement body)
            {
                _body = body;
            }

            public void OnWriteBodyContents(XmlWriter writer)
            {
                _body.WriteTo(writer);
            }
        }
    }
}
// ReSharper restore InconsistentNaming
