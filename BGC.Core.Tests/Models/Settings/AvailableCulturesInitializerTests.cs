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
            first.Name[CultureInfo.GetCultureInfo("en-US")] = new ComposerName("John Smith", "en-US");
            first.Name[CultureInfo.GetCultureInfo("bg-BG")] = new ComposerName("John Smith", "bg-BG");

            first.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(first, first.Name[CultureInfo.GetCultureInfo("en-US")], CultureInfo.GetCultureInfo("en-US")),
                new ComposerArticle(first, first.Name[CultureInfo.GetCultureInfo("bg-BG")], CultureInfo.GetCultureInfo("bg-BG"))
            };

            Composer second = new Composer();
            second.Name[CultureInfo.GetCultureInfo("en-US")] = new ComposerName("Jake Smith", "en-US");
            second.Name[CultureInfo.GetCultureInfo("bg-BG")] = new ComposerName("Jake Smith", "bg-BG");
            second.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(second, second.Name[CultureInfo.GetCultureInfo("bg-BG")], CultureInfo.GetCultureInfo("bg-BG")),
                new ComposerArticle(second, second.Name[CultureInfo.GetCultureInfo("en-US")], CultureInfo.GetCultureInfo("en-US"))
            };

            Composer third = new Composer();
            third.Name[CultureInfo.GetCultureInfo("en-US")] = new ComposerName("Jim Smith", "en-US");
            third.Name[CultureInfo.GetCultureInfo("bg-BG")] = new ComposerName("Jim Smith", "bg-BG");
            third.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(third, third.Name[CultureInfo.GetCultureInfo("bg-BG")], CultureInfo.GetCultureInfo("bg-BG")),
                new ComposerArticle(third, third.Name[CultureInfo.GetCultureInfo("en-US")], CultureInfo.GetCultureInfo("en-US"))
            };

            var composerRepo = new List<Composer>() { first, second, third };

            AvailableCulturesInitializer availableCultures = new AvailableCulturesInitializer(new MultiCultureInfoSetting("Any"), GetMockRepository(composerRepo).Object);
            MultiCultureInfoSetting setting = availableCultures.Initialize();

            Assert.AreEqual(2, setting.Cultures.Count());
        }

        [Test]
        public void IdentifiesCorrectCultures_SomeCulturesMissing()
        {
            Composer first = new Composer();
            first.Name[CultureInfo.GetCultureInfo("en-US")] = new ComposerName("John Smith", "en-US");
            first.Name[CultureInfo.GetCultureInfo("bg-BG")] = new ComposerName("John Smith", "bg-BG");
            first.Name[CultureInfo.GetCultureInfo("de-DE")] = new ComposerName("John Smith", "de-DE");
            first.Name[CultureInfo.GetCultureInfo("fr-FR")] = new ComposerName("John Smith", "fr-FR");
            first.Name[CultureInfo.GetCultureInfo("en-GB")] = new ComposerName("John Smith", "en-GB");
            first.Name[CultureInfo.GetCultureInfo("ja-JP")] = new ComposerName("John Smith", "ja-JP");

            first.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(first, first.Name[CultureInfo.GetCultureInfo("en-US")], CultureInfo.GetCultureInfo("en-US")),
                new ComposerArticle(first, first.Name[CultureInfo.GetCultureInfo("bg-BG")], CultureInfo.GetCultureInfo("bg-BG")),
                new ComposerArticle(first, first.Name[CultureInfo.GetCultureInfo("fr-FR")], CultureInfo.GetCultureInfo("fr-FR")),
            };                                                                

            Composer second = new Composer();
            second.Name[CultureInfo.GetCultureInfo("bg-BG")] = new ComposerName("Jake Smith", "bg-BG");
            second.Name[CultureInfo.GetCultureInfo("en-US")] = new ComposerName("Jake Smith", "en-US");
            second.Name[CultureInfo.GetCultureInfo("ja-JP")] = new ComposerName("Jake Smith", "ja-JP");
            second.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(second, second.Name[CultureInfo.GetCultureInfo("bg-BG")], CultureInfo.GetCultureInfo("bg-BG")),
                new ComposerArticle(second, second.Name[CultureInfo.GetCultureInfo("en-US")], CultureInfo.GetCultureInfo("en-US")),
                new ComposerArticle(second, second.Name[CultureInfo.GetCultureInfo("ja-JP")], CultureInfo.GetCultureInfo("ja-JP"))
            };

            Composer third = new Composer();
            third.Name[CultureInfo.GetCultureInfo("bg-BG")] = new ComposerName("Jim Smith", "bg-BG");
            third.Name[CultureInfo.GetCultureInfo("en-US")] = new ComposerName("Jim Smith", "en-US");
            third.Name[CultureInfo.GetCultureInfo("en-GB")] = new ComposerName("Jim Smith", "en-GB");
            third.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(third, third.Name[CultureInfo.GetCultureInfo("bg-BG")], CultureInfo.GetCultureInfo("bg-BG")),
                new ComposerArticle(third, third.Name[CultureInfo.GetCultureInfo("en-US")], CultureInfo.GetCultureInfo("en-US")),
                new ComposerArticle(third, third.Name[CultureInfo.GetCultureInfo("en-GB")], CultureInfo.GetCultureInfo("en-GB")),
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
            first.Name[CultureInfo.GetCultureInfo("en-US")] = new ComposerName("John Smith", "en-US");
            first.Name[CultureInfo.GetCultureInfo("bg-BG")] = new ComposerName("John Smith", "bg-BG");
            first.Name[CultureInfo.GetCultureInfo("de-DE")] = new ComposerName("John Smith", "de-DE");
            first.Name[CultureInfo.GetCultureInfo("fr-FR")] = new ComposerName("John Smith", "fr-FR");
            first.Articles = new List<ComposerArticle>() { new ComposerArticle(first, first.Name[CultureInfo.GetCultureInfo("en-US")], CultureInfo.GetCultureInfo("en-US")) };

            Composer second = new Composer();
            second.Name[CultureInfo.GetCultureInfo("bg-BG")] = new ComposerName("Jack Smith", "bg-BG");
            second.Name[CultureInfo.GetCultureInfo("fr-FR")] = new ComposerName("Jack Smith", "fr-FR");
            second.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(second, second.Name[CultureInfo.GetCultureInfo("bg-BG")], CultureInfo.GetCultureInfo("bg-BG")),
                new ComposerArticle(second, second.Name[CultureInfo.GetCultureInfo("fr-FR")], CultureInfo.GetCultureInfo("fr-FR"))
            };

            Composer third = new Composer();
            third.Name[CultureInfo.GetCultureInfo("de-DE")] = new ComposerName("Jim Smith", "de-DE");
            third.Articles = new List<ComposerArticle>() { new ComposerArticle(third, third.Name[CultureInfo.GetCultureInfo("de-DE")], CultureInfo.GetCultureInfo("de-DE")) };

            var composerRepo = new List<Composer>() { first, second, third };

            AvailableCulturesInitializer availableCultures = new AvailableCulturesInitializer(new MultiCultureInfoSetting("Any"), GetMockRepository(composerRepo).Object);
            MultiCultureInfoSetting setting = availableCultures.Initialize();

            Assert.AreEqual(0, setting.Cultures.Count());
        }
    }
}
