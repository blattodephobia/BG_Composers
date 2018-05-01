using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using TestUtils;

namespace BGC.Core.Tests.Models.Settings.HtmlDocumentSettingTests
{
    public class HtmlEncodedStringSettingTests
    {
        private static readonly string CommonHtml = "<html><head></head><body></body></html>";
        private static readonly string EncodedCommonHtml = "&lt;html&gt;&lt;head&gt;&lt;/head&gt;&lt;body&gt;&lt;/body&gt;&lt;/html&gt;";
        private static readonly string HtmlNodes = "<p>Hello</p><p>World</p>";
        private static readonly string EncodedHtmlNodes = "&lt;p&gt;Hello&lt;/p&gt;&lt;p&gt;World&lt;/p&gt;";

        public class HtmlStringTests : TestFixtureBase
        {
            [Test]
            public void SetsBaseStringValueWhenNull()
            {
                var setting = new HtmlEncodedStringSetting("setting");
                setting.HtmlString = new System.Web.HtmlString(CommonHtml);

                Assert.IsNotNull(setting.StringValue);

                setting.HtmlString = null;

                Assert.IsNull(setting.StringValue);
            }

            [Test]
            public void SetsXmlDocumentWhenNull()
            {
                var setting = new HtmlEncodedStringSetting("setting");
                setting.HtmlString = new HtmlString(CommonHtml);

                Assert.IsNotNull(setting.Document);

                setting.HtmlString = null;

                Assert.IsNull(setting.Document);
            }

            [Test]
            public void SetsBaseStringValueCorrectly()
            {
                var setting = new HtmlEncodedStringSetting("setting");
                setting.HtmlString = new HtmlString(CommonHtml);

                Assert.AreEqual(EncodedCommonHtml, setting.StringValue);
            }

            [Test]
            public void SetsXmlDocumentCorrectly()
            {
                var setting = new HtmlEncodedStringSetting("setting");
                setting.HtmlString = new HtmlString(CommonHtml);

                Assert.AreEqual("html", setting.Document.DocumentElement.Name);
            }
        }

        public class StringValueTests : TestFixtureBase
        {
            [Test]
            public void EncodesStringValue()
            {
                var setting = new HtmlEncodedStringSetting("setting");
                setting.StringValue = CommonHtml;

                Assert.AreEqual(EncodedCommonHtml, setting.StringValue);
            }

            [Test]
            public void DoesntReEncodeEncodedStrings()
            {
                var setting = new HtmlEncodedStringSetting("setting");
                setting.StringValue = EncodedCommonHtml;

                Assert.AreEqual(EncodedCommonHtml, setting.StringValue);
            }

            [Test]
            public void SetsHtmlStringWhenNullValue()
            {
                var setting = new HtmlEncodedStringSetting("setting");
                setting.StringValue = null;

                Assert.IsNull(setting.HtmlString);
            }

            [Test]
            public void SetsXmlDocumentWhenNullValue()
            {
                var setting = new HtmlEncodedStringSetting("setting");
                setting.StringValue = null;

                Assert.IsNull(setting.Document);

            }

            [Test]
            public void SetsHtmlStringCorrectly()
            {
                var setting = new HtmlEncodedStringSetting("setting");
                setting.StringValue = CommonHtml;

                Assert.AreEqual(EncodedCommonHtml, setting.HtmlString.ToHtmlString());
            }

            [Test]
            public void SetsXmlDocumentCorrectly()
            {
                var setting = new HtmlEncodedStringSetting("setting");
                setting.StringValue = CommonHtml;

                Assert.AreEqual("html", setting.Document.DocumentElement.Name);
            }
        }

        public class XmlDocumentTests : TestFixtureBase
        {
            [Test]
            public void SetsHtmlStringWhenNullString()
            {
                var setting = new HtmlEncodedStringSetting("setting");

                var doc = new XmlDocument();
                doc.LoadXml(CommonHtml);
                setting.Document = doc;

                Assert.IsNotNull(setting.HtmlString);

                setting.Document = null;
                Assert.IsNull(setting.HtmlString);
            }

            [Test]
            public void SetsStringValueWhenNullString()
            {
                var setting = new HtmlEncodedStringSetting("setting");

                var doc = new XmlDocument();
                doc.LoadXml(CommonHtml);
                setting.Document = doc;

                Assert.IsNotNull(setting.StringValue);

                setting.Document = null;
                Assert.IsNull(setting.StringValue);
            }

            [Test]
            public void SetsHtmlStringCorrectly()
            {
                var setting = new HtmlEncodedStringSetting("setting");

                var doc = new XmlDocument();
                doc.LoadXml(CommonHtml);
                setting.Document = doc;

                Assert.AreEqual(EncodedCommonHtml, setting.HtmlString.ToHtmlString());
            }

            [Test]
            public void SetsStringValueCorrectly()
            {
                var setting = new HtmlEncodedStringSetting("setting");

                var doc = new XmlDocument();
                doc.LoadXml(CommonHtml);
                setting.Document = doc;

                Assert.AreEqual(EncodedCommonHtml, setting.StringValue);
            }
        }

        public class CtorTests : TestFixtureBase
        {
            [Test]
            public void InitializesValueCorrectly()
            {
                var setting = new HtmlEncodedStringSetting("setting", CommonHtml);

                Assert.AreEqual(EncodedCommonHtml, setting.HtmlString.ToHtmlString());
            }
        }

        public class ImproperXmlTests : TestFixtureBase
        {
            [Test]
            public void SetsDocumentToNullIfNoRoot()
            {
                var setting = new HtmlEncodedStringSetting("setting", HtmlNodes);

                Assert.IsNull(setting.Document);
                Assert.AreEqual(EncodedHtmlNodes, setting.HtmlString.ToHtmlString());
            }

            [Test]
            public void SetsNodesCorrectly()
            {
                var setting = new HtmlEncodedStringSetting("setting", HtmlNodes);

                Assert.IsNull(setting.Document);

                Assert.AreEqual(2, setting.Nodes.Count());
                Assert.AreEqual("p", setting.Nodes.First().Name);
                Assert.AreEqual("p", setting.Nodes.Last().Name);
            }
        }

        public class XmlDocumentChangedTests : TestFixtureBase
        {
            [Test]
            public void SetsNodesCorrectlyWithRoot()
            {
                var setting = new HtmlEncodedStringSetting("setting", CommonHtml);

                Assert.AreEqual(1, setting.Nodes.Count());
                Assert.AreEqual("html", setting.Nodes.First().Name);
            }

            [Test]
            public void UpdatesNodesIfXmlDocumentChanged()
            {
                var setting = new HtmlEncodedStringSetting("setting", CommonHtml);

                XmlDocument doc = setting.Document;
                doc.DocumentElement.AppendChild(doc.CreateNode(XmlNodeType.Element, "footer", doc.NamespaceURI));

                Assert.AreEqual(3, setting.Nodes.Count());
                Assert.AreEqual("footer", setting.Nodes.Last().Name);
            }
        }
    }
}
