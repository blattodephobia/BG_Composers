using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models
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
}
