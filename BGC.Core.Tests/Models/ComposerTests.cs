using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models
{
    [TestClass]
    public class ComposerTests
    {
        [TestClass]
        public class GetArticleTests
        {
            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsExceptionIfArticleNotFound()
            {
                Composer c = new Composer();
                c.Articles = new List<ComposerArticle>() { new ComposerArticle() { LanguageInternal = "bg-BG" } };
                c.GetArticle(CultureInfo.GetCultureInfo("en-US"));
            }
        }
    }
}
