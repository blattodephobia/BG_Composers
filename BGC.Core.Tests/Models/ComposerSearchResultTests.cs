using BGC.Core.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Core.Models.ComposerSearchResultTests
{
    public class CtorTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullLocale()
        {
            Assert.Throws<ArgumentNullException>(() => new ComposerSearchResult(new Composer(), null));
        }

        [Test]
        public void ThrowsExceptionIfNullComposer()
        {
            Assert.Throws<ArgumentNullException>(() => new ComposerSearchResult(null, CultureInfo.GetCultureInfo("en-US")));
        }

        [Test]
        public void ThrowsExceptionIfNoLocalizedName()
        {
            var entity = new Composer();
            var locale = CultureInfo.GetCultureInfo("en-US");
            entity.AddArticle(new ComposerArticle(entity, new ComposerName("John Smith", locale), locale));
            Assert.Throws<NameNotFoundException>(() => new ComposerSearchResult(entity, locale));
        }

        [Test]
        public void ThrowsExceptionIfNoLocalizedArticle()
        {
            var entity = new Composer();
            var locale = CultureInfo.GetCultureInfo("en-US");
            entity.Name[locale] = new ComposerName("John Smith", locale);
            Assert.Throws<ArticleNotFoundException>(() => new ComposerSearchResult(entity, locale));
        }

        [Test]
        public void SetsPropertiesCorrectly()
        {
            var entity = new Composer();
            var locale = CultureInfo.GetCultureInfo("en-US");
            var name = new ComposerName("John Smith", locale);
            entity.Name[locale] = name;
            entity.AddArticle(new ComposerArticle(entity, name, locale));
            entity.Profile.ProfilePicture = new MediaTypeInfo("image/jpeg") { StorageId = new Guid(1, 2, 3, new byte[8]) };

            var searchResult = new ComposerSearchResult(entity, locale);
            Assert.AreSame(searchResult.Name, entity.Name[locale], "Name wasn't set correctly");
            Assert.AreSame(searchResult.ArticlePreview, entity.FindArticle(locale), "Article wasn't set correctly");
            Assert.AreSame(searchResult.Preview, entity.Profile.ProfilePicture, "Profile picture wasn't set correctly");
        }
    }

    public class NameTests : TestFixtureBase
    {
        private readonly ComposerSearchResult _testResult;

        public NameTests()
        {
            var entity = new Composer();
            var locale = CultureInfo.GetCultureInfo("en-US");
            var name = new ComposerName("John Smith", locale);
            entity.Name[locale] = name;
            entity.AddArticle(new ComposerArticle(entity, name, locale));
            entity.Profile.ProfilePicture = new MediaTypeInfo("image/jpeg") { StorageId = new Guid(1, 2, 3, new byte[8]) };

            _testResult = new ComposerSearchResult(entity, locale);
        }

        [Test]
        public void WrapsHeaderProperty()
        {
            Assert.AreEqual(_testResult.Name.FullName, _testResult.Header);
        }

        [Test]
        public void DoesntChangeNameWhenHeaderIsSet()
        {
            _testResult.Header = "Some string";
            Assert.AreEqual(_testResult.Name.FullName, _testResult.Header);
        }

        [Test]
        public void DoesntChangeHeader()
        {
            string oldHeader = _testResult.Header;
            _testResult.Header = "Another string";
            Assert.AreEqual(oldHeader, _testResult.Header);
        }
    }
}
