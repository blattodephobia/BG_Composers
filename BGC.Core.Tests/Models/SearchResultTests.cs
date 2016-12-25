using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BGC.Core.Tests.Models
{
    [TestFixture]
    public class SearchResult_CtorTests
    {
        [Test]
        public void InitializesWithGuid()
        {
            var initialId = new Guid(4, 1, 1, new byte[8]);
            var result = new SearchResult(initialId);
            var currentId = new Guid(result.Id);
            Assert.AreEqual(initialId, currentId);
        }

        [Test]
        public void InitializesWithInt32_1()
        {
            var result = new SearchResult(4);
            Assert.AreEqual(4, result.Id[0]);
        }

        [Test]
        public void InitializesWithInt32_2()
        {
            var result = new SearchResult(0x0A0000BB);
            Assert.AreEqual(0xBB, result.Id[0]);
            Assert.AreEqual(0x0A, result.Id[3]);
        }
    }

    [TestFixture]
    public class SearchResult_XmlContentTests
    {
        [Test]
        public void SetsParsedXmlProperty()
        {
            var result = new SearchResult();
            Assert.IsNull(result.ParsedResultXml);

            result.ResultXml = @"<root><child/></root>";
            Assert.AreEqual("root", result.ParsedResultXml.DocumentElement.Name);
            Assert.AreEqual("child", result.ParsedResultXml.DocumentElement.FirstChild.Name);
        }

        [Test]
        public void ResetsParsedXmlProperty()
        {
            var result = new SearchResult();
            Assert.IsNull(result.ParsedResultXml);

            result.ResultXml = @"<root><child/></root>";
            Assert.IsNotNull(result.ParsedResultXml);

            result.ResultXml = null;
            Assert.IsNull(result.ParsedResultXml);
        }

        [Test]
        public void SetsXmlPropertyFromParsedXml()
        {
            var parsedXml = new XmlDocument();
            parsedXml.LoadXml(@"<root><child/></root>");

            var result = new SearchResult() { ParsedResultXml = parsedXml };
            Assert.AreEqual(@"<root><child /></root>", result.ResultXml);
        }

        [Test]
        public void ResetsXmlPropertyFromParsedXml()
        {
            var parsedXml = new XmlDocument();
            parsedXml.LoadXml(@"<root><child/></root>");

            var result = new SearchResult() { ParsedResultXml = parsedXml };
            Assert.IsNotNull(result.ResultXml);

            result.ParsedResultXml = null;
            Assert.IsNull(result.ResultXml);
        }
    }
}
