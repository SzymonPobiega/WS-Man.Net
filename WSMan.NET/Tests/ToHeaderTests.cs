using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using WSMan.NET.Addressing;
using WSMan.NET.Transfer;

// ReSharper disable InconsistentNaming
namespace WSMan.NET
{
    [TestFixture]
    public class ToHeaderTests
    {
        [Test]
        public void It_returns_URI_as_content()
        {
            var header = new ToHeader("http://example.com");

            var content = header.Write();

            Assert.AreEqual(1, content.Count());
            var textNode = content.First() as XText;
            Assert.AreEqual("http://example.com", textNode.Value);
        }

        [Test]
        public void It_reads_URI_from_content()
        {
            var content = new List<XNode>
                              {
                                  new XText("http://example.com")
                              };
            var header = new ToHeader();

            header.Read(content);

            Assert.AreEqual("http://example.com", header.Uri);
        }
    }
}
// ReSharper restore InconsistentNamingx
