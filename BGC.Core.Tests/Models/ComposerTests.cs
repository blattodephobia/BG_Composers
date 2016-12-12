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
            Composer c = new Composer();
            c.Articles = new List<ComposerArticle>() { new ComposerArticle() { LanguageInternal = "bg-BG" } };
            Assert.IsNull(c.GetArticle(CultureInfo.GetCultureInfo("en-US")));
        }
    }
}
