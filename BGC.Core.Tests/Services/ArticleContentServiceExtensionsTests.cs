using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestUtils;
using static TestUtils.MockUtilities;

namespace BGC.Core.Services.ArticleContentServiceExtensionsTests
{
    public class GetPreviewTests : TestFixtureBase
    {
        private readonly Guid _mockArticleKey;
        private readonly Dictionary<Guid, string> _articleRepo;
        private readonly Mock<IArticleContentService> _articleSvc;

        private string TestXml
        {
            get
            {
                return _articleRepo[_mockArticleKey];
            }

            set
            {
                _articleRepo[_mockArticleKey] = value;
            }
        }

        public GetPreviewTests()
        {
            _mockArticleKey = default(Guid);
            _articleRepo = new Dictionary<Guid, string>() { { _mockArticleKey, "" } };
            _articleSvc = GetMockArticleService(_articleRepo);
        }

        public override void BeforeEachTest()
        {
            base.BeforeEachTest();

            TestXml = string.Empty;
        }

        [Test]
        public void ThrowsExceptionIfNullService()
        {
            Assert.Throws<ArgumentNullException>(() => ArticleContentServiceExtensions.GeneratePreview(null, _mockArticleKey));
        }

        [Test]
        public void ThrowsExceptionIfMalformedXml()
        {
            TestXml = $"<p></p><p></p"; // closing tag misses the '>' character

            Assert.Throws<XmlException>(() => _articleSvc.Object.GeneratePreview(_mockArticleKey));
        }

        [Test]
        public void GetsPreview_MultipleRootElements()
        {
            string previewText = "HI";
            TestXml = $"<p>{previewText}</p><p></p>";

            Assert.AreEqual(previewText, _articleSvc.Object.GeneratePreview(_mockArticleKey).InnerText);
        }

        [Test]
        public void GetsPreview_SingleRootElement()
        {
            string previewText = "HI";
            TestXml = $"<root><p>{previewText}</p><p></p></root>";

            Assert.AreEqual(previewText, _articleSvc.Object.GeneratePreview(_mockArticleKey).InnerText);
        }

        [Test]
        public void SanitizesImages()
        {
            string previewText = "HI";
            TestXml = $"<root><p><img />{previewText}</p><p></p></root>";

            XmlNode preview = _articleSvc.Object.GeneratePreview(_mockArticleKey);;
            Assert.IsTrue(preview.CreateNavigator().SelectSingleNode("//img") == null);
        }

        [Test]
        public void SanitizesScripts()
        {
            TestXml = $"<root><p><div><script></script></div>Hello world!</p><p></p></root>";

            XmlNode preview = _articleSvc.Object.GeneratePreview(_mockArticleKey);;
            Assert.IsTrue(preview.CreateNavigator().SelectSingleNode("//script") == null);
        }

        [Test]
        public void ReturnsNullIfNoArticle()
        {
            TestXml = null;

            XmlNode preview = _articleSvc.Object.GeneratePreview(_mockArticleKey);
            Assert.IsNull(preview);
        }
    }
}
