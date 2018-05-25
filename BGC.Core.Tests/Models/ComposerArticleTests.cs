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

    public class EqualsTest : TestFixtureBase
    {
        [Test]
        public void EqualsOther()
        {
            var id1 = new Guid(1, 0, 0, new byte[8]);
            var id2 = new Guid(id1.ToByteArray());

            var article1 = new ComposerArticle(new Composer() { Id = id1 }, new ComposerName("Sun Yu", "en-US"), CultureInfo.GetCultureInfo("en-US"))
            {
                IsArchived = true,
                StorageId = id1,
                CreatedUtc = new DateTime(2001, 1, 2),
            };
            var article2 = new ComposerArticle(new Composer() { Id = id2 }, new ComposerName("Sun Yu", "en-US"), CultureInfo.GetCultureInfo("en-US"))
            {
                IsArchived = true,
                StorageId = id2,
                CreatedUtc = new DateTime(2001, 1, 2),
            };

            Assert.AreEqual(article1, article2);
        }

        [Test]
        public void NotEqualsNull()
        {
            var id1 = new Guid(1, 0, 0, new byte[8]);

            var article1 = new ComposerArticle(new Composer() { Id = id1 }, new ComposerName("Sun Yu", "en-US"), CultureInfo.GetCultureInfo("en-US"))
            {
                IsArchived = true,
                StorageId = id1,
                CreatedUtc = new DateTime(2001, 1, 2),
            };

            Assert.AreNotEqual(null, article1);
            Assert.AreNotEqual(article1, null);
        }

        [Test]
        public void EqualsSelf()
        {
            var id1 = new Guid(1, 0, 0, new byte[8]);

            var article1 = new ComposerArticle(new Composer() { Id = id1 }, new ComposerName("Sun Yu", "en-US"), CultureInfo.GetCultureInfo("en-US"))
            {
                IsArchived = true,
                StorageId = id1,
                CreatedUtc = new DateTime(2001, 1, 2),
            };
            
            Assert.AreEqual(article1, article1);
        }

        [Test]
        public void NotEqualsOther()
        {
            var id1 = new Guid(1, 0, 0, new byte[8]);
            var id2 = new Guid(2, 2, 2, new byte[8]);

            var article1 = new ComposerArticle(new Composer() { Id = id1 }, new ComposerName("Sun Yu", "en-US"), CultureInfo.GetCultureInfo("en-US"))
            {
                IsArchived = true,
                StorageId = id1,
                CreatedUtc = new DateTime(2001, 1, 2),
            };
            var article2 = new ComposerArticle(new Composer() { Id = id1 }, new ComposerName("Sun Yu", "en-US"), CultureInfo.GetCultureInfo("en-US"))
            {
                IsArchived = true,
                StorageId = id1,
                CreatedUtc = new DateTime(2001, 1, 2),
            };

            Assert.AreEqual(article1, article2);
            article2.StorageId = id2;
            Assert.AreNotEqual(article1, article2);
            article2.StorageId = id1;

            Assert.AreEqual(article1, article2);
            article2.CreatedUtc = new DateTime();
            Assert.AreNotEqual(article1, article2);
            article2.CreatedUtc = article1.CreatedUtc;

            Assert.AreEqual(article1, article2);
            article2.IsArchived = false;
            Assert.AreNotEqual(article1, article2);
            article2.IsArchived = true;

            Assert.AreEqual(article1, article2);
            article2.Composer.Id = id2;
            Assert.AreNotEqual(article1, article2);

        }
    }
}
