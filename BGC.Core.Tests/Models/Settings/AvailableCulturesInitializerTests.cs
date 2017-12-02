using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;
using static TestUtils.MockUtilities;

namespace BGC.Core.Tests.Models.Settings.AvailableCulturesInitializerTests
{
    public class InitializeTests : TestFixtureBase
    {
        [Test]
        public void IdentifiesCorrectCultures_ConsistentDb()
        {
            Composer first = new Composer();
            first.Articles = new List<ComposerArticle>() { new ComposerArticle(first, CultureInfo.GetCultureInfo("en-US")), new ComposerArticle(first, CultureInfo.GetCultureInfo("bg-BG")) };

            Composer second = new Composer();
            second.Articles = new List<ComposerArticle>() { new ComposerArticle(second, CultureInfo.GetCultureInfo("bg-BG")), new ComposerArticle(second, CultureInfo.GetCultureInfo("en-US")) };

            Composer third = new Composer();
            third.Articles = new List<ComposerArticle>() { new ComposerArticle(third, CultureInfo.GetCultureInfo("bg-BG")), new ComposerArticle(third, CultureInfo.GetCultureInfo("en-US")) };

            var composerRepo = new List<Composer>() { first, second, third };

            AvailableCulturesInitializer availableCultures = new AvailableCulturesInitializer(new MultiCultureInfoSetting("Any"), GetMockRepository(composerRepo).Object);
            MultiCultureInfoSetting setting = availableCultures.Initialize();

            Assert.AreEqual(2, setting.Cultures.Count());
        }

        [Test]
        public void IdentifiesCorrectCultures_SomeCulturesMissing()
        {
            Composer first = new Composer();
            first.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(first, CultureInfo.GetCultureInfo("en-US")),
                new ComposerArticle(first, CultureInfo.GetCultureInfo("bg-BG")),
                new ComposerArticle(first, CultureInfo.GetCultureInfo("fr-FR")),
            };

            Composer second = new Composer();
            second.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(second, CultureInfo.GetCultureInfo("bg-BG")),
                new ComposerArticle(second, CultureInfo.GetCultureInfo("en-US")),
                new ComposerArticle(second, CultureInfo.GetCultureInfo("ja-JP"))
            };

            Composer third = new Composer();
            third.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(third, CultureInfo.GetCultureInfo("bg-BG")),
                new ComposerArticle(third, CultureInfo.GetCultureInfo("en-US")),
                new ComposerArticle(third, CultureInfo.GetCultureInfo("en-GB")),
            };

            var composerRepo = new List<Composer>() { first, second, third };

            AvailableCulturesInitializer availableCultures = new AvailableCulturesInitializer(new MultiCultureInfoSetting("Any"), GetMockRepository(composerRepo).Object);
            MultiCultureInfoSetting setting = availableCultures.Initialize();

            Assert.AreEqual(2, setting.Cultures.Count());
        }

        [Test]
        public void IdentifiesCorrectCultures_AllCulturesInconsistent()
        {
            Composer first = new Composer();
            first.Articles = new List<ComposerArticle>() { new ComposerArticle(first, CultureInfo.GetCultureInfo("en-US")) };

            Composer second = new Composer();
            second.Articles = new List<ComposerArticle>() { new ComposerArticle(second, CultureInfo.GetCultureInfo("bg-BG")), new ComposerArticle(second, CultureInfo.GetCultureInfo("fr-FR")) };

            Composer third = new Composer();
            third.Articles = new List<ComposerArticle>() { new ComposerArticle(third, CultureInfo.GetCultureInfo("de-DE")) };

            var composerRepo = new List<Composer>() { first, second, third };

            AvailableCulturesInitializer availableCultures = new AvailableCulturesInitializer(new MultiCultureInfoSetting("Any"), GetMockRepository(composerRepo).Object);
            MultiCultureInfoSetting setting = availableCultures.Initialize();

            Assert.AreEqual(0, setting.Cultures.Count());
        }
    }
}
