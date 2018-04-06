using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using TestUtils;

namespace BGC.Core.Tests.Models.Settings
{
    public class XmlDocumentSettingTests : TestFixtureBase
    {
        private class XmlDocumentTestProxy : XmlDocumentSetting
        {
            public XmlDocumentTestProxy()
            {
            }
        }

        [Test]
        public void StringCtor()
        {
            string xmlString = @"<root></root>";
            var setting = new XmlDocumentSetting(xmlString);

            Assert.AreEqual("root", setting.Document.DocumentElement.Name);
            Assert.AreEqual(xmlString, setting.StringValue);
        }

        [Test]
        public void XmlDocumentCtorTest()
        {
            string xmlString = @"<root></root>";
            var doc = new XmlDocument();
            doc.LoadXml(xmlString);

            var setting = new XmlDocumentSetting(doc);

            Assert.AreEqual("root", setting.Document.DocumentElement.Name);
            Assert.AreEqual(xmlString, setting.StringValue);
        }

        [Test]
        public void ParsesStringValueToDocument()
        {
            var obj = new XmlDocumentTestProxy();

            string xmlString = @"<root></root>";
            obj.StringValue = xmlString;

            Assert.AreEqual("root", obj.Document.DocumentElement.Name);
        }

        [Test]
        public void WritesXmlDocumentToStringValue()
        {
            var obj = new XmlDocumentTestProxy();

            string xmlString = @"<root></root>";
            obj.Document = new XmlDocument();
            obj.Document.LoadXml(xmlString);

            Assert.AreEqual(xmlString, obj.StringValue);
        }

        [Test]
        public void ThrowsExceptionIfNullString()
        {
            string xmlString = @"<root></root>";
            var setting = new XmlDocumentSetting(xmlString);

            Assert.Throws<InvalidOperationException>(() => setting.StringValue = null);
        }

        [Test]
        public void ThrowsExceptionIfEmptyString()
        {
            string xmlString = @"<root></root>";
            var setting = new XmlDocumentSetting(xmlString);

            Assert.Throws<InvalidOperationException>(() => setting.StringValue = " ");
        }

        [Test]
        public void ThrowsExceptionIfNullDocument()
        {
            string xmlString = @"<root></root>";
            var setting = new XmlDocumentSetting(xmlString);

            Assert.Throws<InvalidOperationException>(() => setting.Document = null);
        }

        [Test]
        public void UpdatesStringValueIfXmlDocumentIsUpdated()
        {
            string xmlString = @"<root></root>";
            var setting = new XmlDocumentSetting(xmlString);

            string node = "node";
            XmlNode newNode = setting.Document.CreateNode(XmlNodeType.Element, node, setting.Document.NamespaceURI);
            setting.Document.DocumentElement.AppendChild(newNode);

            Assert.AreEqual(@"<root><node /></root>", setting.StringValue);
        }

        [Test]
        public void DoesntUpdateStringValueForOldXmlDocuments()
        {
            string xmlString = @"<root></root>";
            var setting = new XmlDocumentSetting(xmlString);
            XmlDocument oldDoc = setting.Document;

            string newXmlString = @"<root2></root2>";
            setting.StringValue = newXmlString;

            XmlNode newNode = oldDoc.CreateNode(XmlNodeType.Element, "node", setting.Document.NamespaceURI);
            oldDoc.DocumentElement.AppendChild(newNode);

            Assert.AreEqual(newXmlString, setting.Document.InnerXml);
        }
    }
}
