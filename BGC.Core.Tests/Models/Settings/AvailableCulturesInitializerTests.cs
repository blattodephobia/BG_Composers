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
            first.Name["en-US".ToCultureInfo()] = new ComposerName("John Smith", "en-US".ToCultureInfo());
            first.Name["bg-BG".ToCultureInfo()] = new ComposerName("John Smith", "bg-BG".ToCultureInfo());

            first.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(first, first.Name["en-US".ToCultureInfo()], "en-US".ToCultureInfo()),
                new ComposerArticle(first, first.Name["bg-BG".ToCultureInfo()], "bg-BG".ToCultureInfo())
            };

            Composer second = new Composer();
            second.Name["en-US".ToCultureInfo()] = new ComposerName("Jake Smith", "en-US".ToCultureInfo());
            second.Name["bg-BG".ToCultureInfo()] = new ComposerName("Jake Smith", "bg-BG".ToCultureInfo());
            second.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(second, second.Name["bg-BG".ToCultureInfo()], "bg-BG".ToCultureInfo()),
                new ComposerArticle(second, second.Name["en-US".ToCultureInfo()], "en-US".ToCultureInfo())
            };

            Composer third = new Composer();
            third.Name["en-US".ToCultureInfo()] = new ComposerName("Jim Smith", "en-US".ToCultureInfo());
            third.Name["bg-BG".ToCultureInfo()] = new ComposerName("Jim Smith", "bg-BG".ToCultureInfo());
            third.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(third, third.Name["bg-BG".ToCultureInfo()], "bg-BG".ToCultureInfo()),
                new ComposerArticle(third, third.Name["en-US".ToCultureInfo()], "en-US".ToCultureInfo())
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
            first.Name["en-US".ToCultureInfo()] = new ComposerName("John Smith", "en-US".ToCultureInfo());
            first.Name["bg-BG".ToCultureInfo()] = new ComposerName("John Smith", "bg-BG".ToCultureInfo());
            first.Name["de-DE".ToCultureInfo()] = new ComposerName("John Smith", "de-DE".ToCultureInfo());
            first.Name["fr-FR".ToCultureInfo()] = new ComposerName("John Smith", "fr-FR".ToCultureInfo());
            first.Name["en-GB".ToCultureInfo()] = new ComposerName("John Smith", "en-GB".ToCultureInfo());
            first.Name["ja-JP".ToCultureInfo()] = new ComposerName("John Smith", "ja-JP".ToCultureInfo());

            first.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(first, first.Name["en-US".ToCultureInfo()], "en-US".ToCultureInfo()),
                new ComposerArticle(first, first.Name["bg-BG".ToCultureInfo()], "bg-BG".ToCultureInfo()),
                new ComposerArticle(first, first.Name["fr-FR".ToCultureInfo()], "fr-FR".ToCultureInfo()),
            };                                                                

            Composer second = new Composer();
            second.Name["bg-BG".ToCultureInfo()] = new ComposerName("Jake Smith", "bg-BG".ToCultureInfo());
            second.Name["en-US".ToCultureInfo()] = new ComposerName("Jake Smith", "en-US".ToCultureInfo());
            second.Name["ja-JP".ToCultureInfo()] = new ComposerName("Jake Smith", "ja-JP".ToCultureInfo());
            second.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(second, second.Name["bg-BG".ToCultureInfo()], "bg-BG".ToCultureInfo()),
                new ComposerArticle(second, second.Name["en-US".ToCultureInfo()], "en-US".ToCultureInfo()),
                new ComposerArticle(second, second.Name["ja-JP".ToCultureInfo()], "ja-JP".ToCultureInfo())
            };

            Composer third = new Composer();
            third.Name["bg-BG".ToCultureInfo()] = new ComposerName("Jim Smith", "bg-BG".ToCultureInfo());
            third.Name["en-US".ToCultureInfo()] = new ComposerName("Jim Smith", "en-US".ToCultureInfo());
            third.Name["en-GB".ToCultureInfo()] = new ComposerName("Jim Smith", "en-GB".ToCultureInfo());
            third.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(third, third.Name["bg-BG".ToCultureInfo()], "bg-BG".ToCultureInfo()),
                new ComposerArticle(third, third.Name["en-US".ToCultureInfo()], "en-US".ToCultureInfo()),
                new ComposerArticle(third, third.Name["en-GB".ToCultureInfo()], "en-GB".ToCultureInfo()),
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
            first.Name["en-US".ToCultureInfo()] = new ComposerName("John Smith", "en-US".ToCultureInfo());
            first.Name["bg-BG".ToCultureInfo()] = new ComposerName("John Smith", "bg-BG".ToCultureInfo());
            first.Name["de-DE".ToCultureInfo()] = new ComposerName("John Smith", "de-DE".ToCultureInfo());
            first.Name["fr-FR".ToCultureInfo()] = new ComposerName("John Smith", "fr-FR".ToCultureInfo());
            first.Articles = new List<ComposerArticle>() { new ComposerArticle(first, first.Name["en-US".ToCultureInfo()], "en-US".ToCultureInfo()) };

            Composer second = new Composer();
            second.Name["bg-BG".ToCultureInfo()] = new ComposerName("Jack Smith", "bg-BG".ToCultureInfo());
            second.Name["fr-FR".ToCultureInfo()] = new ComposerName("Jack Smith", "fr-FR".ToCultureInfo());
            second.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(second, second.Name["bg-BG".ToCultureInfo()], "bg-BG".ToCultureInfo()),
                new ComposerArticle(second, second.Name["fr-FR".ToCultureInfo()], "fr-FR".ToCultureInfo())
            };

            Composer third = new Composer();
            third.Name["de-DE".ToCultureInfo()] = new ComposerName("Jim Smith", "de-DE".ToCultureInfo());
            third.Articles = new List<ComposerArticle>() { new ComposerArticle(third, third.Name["de-DE".ToCultureInfo()], "de-DE".ToCultureInfo()) };

            var composerRepo = new List<Composer>() { first, second, third };

            AvailableCulturesInitializer availableCultures = new AvailableCulturesInitializer(new MultiCultureInfoSetting("Any"), GetMockRepository(composerRepo).Object);
            MultiCultureInfoSetting setting = availableCultures.Initialize();

            Assert.AreEqual(0, setting.Cultures.Count());
        }
    }
}
