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

        [Test]
        public void InitializesWithInt64()
        {
            var result = new SearchResult(0x0ABBCC0000DDEEFFL);
            Assert.AreEqual(0xFF, result.Id[0]);
            Assert.AreEqual(0xEE, result.Id[1]);
            Assert.AreEqual(0xDD, result.Id[2]);
            Assert.AreEqual(0x00, result.Id[3]);
            Assert.AreEqual(0x00, result.Id[4]);
            Assert.AreEqual(0xCC, result.Id[5]);
            Assert.AreEqual(0xBB, result.Id[6]);
            Assert.AreEqual(0x0A, result.Id[7]);
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

    [TestFixture]
    public class AsIntTests
    {
        [Test]
        public void ConvertsIdToInt32Correctly()
        {
            var result = new SearchResult(0x0A0B0C0D);
            Assert.AreEqual(0x0A0B0C0D, result.IdAsInt());
        }

        [Test]
        public void ThrowsExceptionIfIdIsNull()
        {
            var result = new SearchResult();
            Assert.Throws<InvalidOperationException>(() => result.IdAsInt());
        }

        [Test]
        public void ThrowsExceptionIfIdIsNot4BytesLong()
        {
            var result = new SearchResult(new byte[3]);
            Assert.Throws<InvalidOperationException>(() => result.IdAsInt());
        }
    }

    [TestFixture]
    public class AsLongTests
    {
        [Test]
        public void ConvertsIdToInt64Correctly()
        {
            var result = new SearchResult(long.MaxValue);
            Assert.AreEqual(long.MaxValue, result.IdAsLong());
        }

        [Test]
        public void ThrowsExceptionIfIdIsNull()
        {
            var result = new SearchResult();
            Assert.Throws<InvalidOperationException>(() => result.IdAsLong());
        }

        [Test]
        public void ThrowsExceptionIfIdIsNot8BytesLong()
        {
            var result = new SearchResult(new byte[6]);
            Assert.Throws<InvalidOperationException>(() => result.IdAsLong());
        }
    }

    [TestFixture]
    public class AsGuidTests
    {
        [Test]
        public void ConvertsIdToGuidCorrectly()
        {
            var id = new Guid(14, 1024, 2056, new byte[8]);
            var result = new SearchResult(id);
            Assert.AreEqual(id, result.IdAsGuid());
        }

        [Test]
        public void ThrowsExceptionIfIdIsNull()
        {
            var result = new SearchResult();
            Assert.Throws<InvalidOperationException>(() => result.IdAsGuid());
        }

        [Test]
        public void ThrowsExceptionIfIdIsNot16BytesLong()
        {
            var result = new SearchResult(new byte[3]);
            Assert.Throws<InvalidOperationException>(() => result.IdAsGuid());
        }
    }
}
