using System.Xml;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using WSMan.NET.SOAP;

// ReSharper disable InconsistentNaming
// ReSharper disable PossibleNullReferenceException
namespace WSMan.NET
{
    [TestFixture]
    public class FaultMessageTests
    {
        [Test]
        public void It_sets_fault_action()
        {
            var message = FaultMessage.Create("someAction", "no reason", FaultCode.Sender, Enumerable.Empty<XmlQualifiedName>());

            var envelope = XElementExtensions.ParseGeneratedXml(message.Write);

            var headerSectionElement = envelope.Element(Constants.Header);
            var actionHeader = headerSectionElement.Element(Addressing.Constants.Namespace + "Action");
            var actionValue = ((XText)actionHeader.FirstNode).Value;
            Assert.AreEqual("someAction", actionValue);
        }

        [Test]
        public void It_sets_fault_code_with_nested_subcodes()
        {
            var message = FaultMessage.Create("someAction", "no reason", FaultCode.Sender, new [] { FaultCode.Receiver, FaultCode.VersionMismatch});

            var envelope = XElementExtensions.ParseGeneratedXml(message.Write);

            var bodyElement = envelope.Element(Constants.Body);
            var codeElement = bodyElement
                .Element(Constants.Namespace + "Fault")
                .Element(Constants.Namespace + "Code");                

            var rootSubcodeElement = codeElement.Element(Constants.Namespace + "Subcode");
            var childSubcodeElement = rootSubcodeElement.Element(Constants.Namespace + "Subcode");

            Assert.AreEqual("Sender", ExtractCodeValue(codeElement));
            Assert.AreEqual("Receiver", ExtractCodeValue(rootSubcodeElement));
            Assert.AreEqual("VersionMismatch", ExtractCodeValue(childSubcodeElement));
        }

        [Test]
        public void It_reads_fault_code_with_nested_subcodes()
        {
            var outgoingMessage = FaultMessage.Create(
                "someAction", 
                "no reason", 
                FaultCode.Sender, 
                new [] {FaultCode.Receiver, FaultCode.VersionMismatch});

            var reader = XElementExtensions.ParseGeneratedXml(outgoingMessage.Write).ToReader();
            var incomingMessage = new IncomingMessage(reader);
            var exception = incomingMessage.CreateFaultException();

            Assert.AreEqual(FaultCode.Sender, exception.Code);
            Assert.IsTrue(new [] {FaultCode.Receiver, FaultCode.VersionMismatch}.SequenceEqual(exception.Subcodes));
        }

        [Test]
        public void It_reads_first_reason()
        {
            var outgoingMessage = FaultMessage.Create(
                "someAction",
                "no reason",
                FaultCode.Sender,
                new[] { FaultCode.Receiver});

            var reader = XElementExtensions.ParseGeneratedXml(outgoingMessage.Write).ToReader();
            var incomingMessage = new IncomingMessage(reader);
            var exception = incomingMessage.CreateFaultException();

            Assert.AreEqual("no reason", exception.Reason);
        }

        private static string ExtractCodeValue(XElement codeElement)
        {
            return ((XText)codeElement.Element(Constants.Namespace + "Value").FirstNode).Value;
        }
    }
}
// ReSharper restore InconsistentNaming
// ReSharper restore PossibleNullReferenceException
