using System;
using System.Xml;
using System.Xml.Linq;
using NUnit.Framework;
using WSMan.NET.SOAP;
using WSMan.NET.Transfer;
using Constants = WSMan.NET.SOAP.Constants;

// ReSharper disable InconsistentNaming
namespace WSMan.NET
{
    [TestFixture]
    public class IncomingMessageTests
    {
        [Test]
        public void It_reads_header_information()
        {
            var messageElement = new XElement(Constants.Envelope,
                                              new XElement(Constants.Header,
                                                           new XElement("SomeHeader",
                                                                        new XText("SomeValue"))),
                                              new XElement(Constants.Body));

            var messageReader = messageElement.ToReader();
            var message = new IncomingMessage(messageReader);

            var header = message.GetHeader(XName.Get("SomeHeader", ""));
            Assert.IsNotNull(header);
        }

        [Test]
        public void It_returns_reader_at_body_contents()
        {
            var messageElement = new XElement(Constants.Envelope,
                                              new XElement(Constants.Header),
                                              new XElement(Constants.Body,
                                                           new XText("Text in the body")));

            var messageReader = messageElement.ToReader();
            var message = new IncomingMessage(messageReader);

            var bodyReader = message.GetReaderAtBodyContents();
            Assert.IsFalse(message.IsEmpty);
            Assert.AreEqual(XmlNodeType.Text, bodyReader.NodeType);
            Assert.AreEqual("Text in the body", bodyReader.Value);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void It_throws_exception_when_trying_to_get_body_reader_when_body_is_empty()
        {
            var messageElement = new XElement(Constants.Envelope,
                                              new XElement(Constants.Header),
                                              new XElement(Constants.Body));

            var messageReader = messageElement.ToReader();
            var message = new IncomingMessage(messageReader);

            Assert.IsTrue(message.IsEmpty);
            var bodyReader = message.GetReaderAtBodyContents();
        }
    }
}
// ReSharper restore InconsistentNaming
