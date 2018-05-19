using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Core.Tests.Models.ComposerArticleTests
{
    public class CtorTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullComposer()
        {
            Assert.Throws<ArgumentNullException>(() => new ComposerArticle(null, new ComposerName("John Smith", "en-US"), new CultureInfo("en-US")));
        }

        [Test]
        public void ThrowsExceptionIfNullName()
        {
            Assert.Throws<ArgumentNullException>(() => new ComposerArticle(new Composer(), null, new CultureInfo("en-US")));
        }

        [Test]
        public void ThrowsExceptionIfNullCulture()
        {
            Assert.Throws<ArgumentNullException>(() => new ComposerArticle(new Composer(), new ComposerName("John Smith", "en-US"), null));
        }

        [Test]
        public void ThrowsExceptionIfCultureMismatch()
        {
            var name = new ComposerName("John Smith", "en-US");
            var culture = new CultureInfo("de-DE");

            Assert.Throws<InvalidOperationException>(() => new ComposerArticle(new Composer(), name, culture));
        }
    }

    public class NameTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullValue()
        {
            var article = new ComposerArticle();

            Assert.Throws<ArgumentNullException>(() => article.LocalizedName = null);
        }

        [Test]
        public void ThrowsExceptionIfCultureMismatch()
        {
            var article = new ComposerArticle() { Language = new CultureInfo("en-US") };

            Assert.Throws<InvalidOperationException>(() => article.LocalizedName = new ComposerName("Jake Gyllenhaal", CultureInfo.GetCultureInfo("de-DE")));
        }

        [Test]
        public void ThrowsExceptionIfComposerMismatch()
        {
            var composer1 = new Composer() { Id = new Guid(0, 0, 1, new byte[8]) };
            var composer2 = new Composer() { Id = new Guid(0, 0, 2, new byte[8]) };

            var name = new ComposerName("Jack Sparrow", "en-US") { Composer = composer1 };
            Assert.Throws<InvalidOperationException>(() => new ComposerArticle(composer2, name, name.Language));
        }
    }
}
