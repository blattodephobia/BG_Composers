using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models.ComposerTests
{
    [TestFixture]
    public class GetArticleTests
    {
        [Test]
        public void ReturnsNullIfArticleNotFound()
        {
            var c = new Composer();
            c.Articles = new List<ComposerArticle>() { new ComposerArticle() { LanguageInternal = "bg-BG" } };
            Assert.IsNull(c.GetArticle(CultureInfo.GetCultureInfo("en-US")));
        }

        [Test]
        public void GetsCorrectArticle_CachedCultureInfo()
        {
            var c = new Composer();
            var bgArticle = new ComposerArticle() { LanguageInternal = "bg-BG" };
            c.Articles = new List<ComposerArticle>() { bgArticle };

            Assert.AreEqual(bgArticle, c.GetArticle(CultureInfo.GetCultureInfo("bg-BG")));
        }

        [Test]
        public void GetsCorrectArticle_AnyCultureInfoInstance()
        {
            var c = new Composer();
            var bgArticle = new ComposerArticle() { LanguageInternal = "bg-BG" };
            c.Articles = new List<ComposerArticle>() { bgArticle };

            Assert.AreEqual(bgArticle, c.GetArticle(new CultureInfo("bg-BG")));
        }

        [Test]
        public void GetsMostRecentVersionOfArticle()
        {
            var c = new Composer();
            var mostRecent = new ComposerArticle() { Language = new CultureInfo("bg-BG"), CreatedUtc = new DateTime(2000, 12, 1) };
            c.Articles = new List<ComposerArticle>()
            {
                mostRecent,
                new ComposerArticle() { Language = new CultureInfo("bg-BG"), CreatedUtc = new DateTime(2000, 11, 1), IsArchived = true, },
                new ComposerArticle() { Language = new CultureInfo("en-US"), CreatedUtc = new DateTime(2000, 12, 1) },
                new ComposerArticle() { Language = new CultureInfo("en-US"), CreatedUtc = new DateTime(2000, 11, 1), IsArchived = true, },

            };

            Assert.AreSame(mostRecent, c.GetArticle(new CultureInfo("bg-BG")));
        }
    }

    [TestFixture]
    public class GetArticlesTests
    {
        [Test]
        public void GetsNonArchivedArticlesOnly()
        {
            var c = new Composer();
            c.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(c, CultureInfo.GetCultureInfo("en-US")) { IsArchived = true },
                new ComposerArticle(c, CultureInfo.GetCultureInfo("en-US")) { IsArchived = true },
                new ComposerArticle(c, CultureInfo.GetCultureInfo("en-US")) { IsArchived = false },
                new ComposerArticle(c, CultureInfo.GetCultureInfo("en-US")) { IsArchived = true },
            };

            Assert.AreSame(c.Articles.ElementAt(2), c.GetArticles().Single());
        }
    }

    [TestFixture]
    public class LocalizedNamesTests
    {
        [Test]
        public void SetsPrincipalEntityToSelf()
        {
            var c = new Composer();
            c.LocalizedNames = new List<ComposerName>() { new ComposerName("John", "en-US") };

            Assert.AreEqual(c, c.LocalizedNames.First().Composer);
        }

        [Test]
        public void ThrowsExceptionIfAnotherComposersNameIsAdded()
        {
            var c = new Composer();
            Assert.Throws<InvalidOperationException>(() =>
            {
                c.LocalizedNames = new List<ComposerName>() { new ComposerName("John", "en-US") { Composer = new Composer() } };
            });
        }
    }

    [TestFixture]
    public class GetNameTests
    {
        [Test]
        public void FindsCorrectName_CachedCultureInfo()
        {
            var c = new Composer();
            var germanName = new ComposerName("John", "de-DE");
            c.LocalizedNames = new List<ComposerName>() { germanName };

            ComposerName actualName = c.GetName(CultureInfo.GetCultureInfo("de-DE"));

            Assert.AreEqual(germanName, actualName);
        }

        [Test]
        public void FindsCorrectName_AnyCultureInfoInstance()
        {
            var c = new Composer();
            var germanName = new ComposerName("John", "de-DE");
            c.LocalizedNames = new List<ComposerName>() { germanName };

            ComposerName actualName = c.GetName(new CultureInfo("de-DE"));

            Assert.AreEqual(germanName, actualName);
        }
    }

    [TestFixture]
    public class IdTests
    {
        [Test]
        public void ThrowsExceptionIfGetHashCodeIsCalled()
        {
            var composer = new Composer();

            var id = new byte[16];
            id[15] = 2;
            composer.Id = new Guid(id);

            composer.GetHashCode();

            Assert.Throws<InvalidOperationException>(() => composer.Id = Guid.NewGuid());
        }
    }

    [TestFixture]
    public class AddArticleTests
    {
        [Test]
        public void ThrowsExceptionIfArticleIsArchived()
        {
            var composer = new Composer();
            Assert.Throws<InvalidOperationException>(() => composer.AddArticle(new ComposerArticle() { IsArchived = true }));
        }

        [Test]
        public void ArchivesSimilarArticles()
        {
            var composer = new Composer();
            var englishArticle = new ComposerArticle() { Language = CultureInfo.GetCultureInfo("en-US") };
            var oldBulgarianArticle = new ComposerArticle() { Language = CultureInfo.GetCultureInfo("bg-BG") };
            composer.Articles.Add(englishArticle);
            composer.Articles.Add(oldBulgarianArticle);

            Assert.IsTrue(composer.Articles.All(a => !a.IsArchived));

            var newBulgarianArticle = new ComposerArticle() { Language = CultureInfo.GetCultureInfo("bg-BG") };
            composer.AddArticle(newBulgarianArticle);
            
            Assert.IsTrue(oldBulgarianArticle.IsArchived);
            Assert.IsFalse(newBulgarianArticle.IsArchived);
        }

        [Test]
        public void DoesntModifyDissimilarArticles()
        {
            var composer = new Composer();
            var englishArticle = new ComposerArticle() { Language = CultureInfo.GetCultureInfo("en-US") };
            var oldBulgarianArticle = new ComposerArticle() { Language = CultureInfo.GetCultureInfo("bg-BG") };
            composer.Articles.Add(englishArticle);
            composer.Articles.Add(oldBulgarianArticle);

            Assert.IsTrue(composer.Articles.All(a => !a.IsArchived));

            var newBulgarianArticle = new ComposerArticle() { Language = CultureInfo.GetCultureInfo("bg-BG") };
            composer.AddArticle(newBulgarianArticle);

            Assert.IsFalse(englishArticle.IsArchived);
        }
    }
}
