using System;
using NUnit.Framework;
using System.Xml;
using System.IO;
using System.Linq;
using System.Threading;
using System.Globalization;

namespace BGC.Services.Tests
{
    public class Helpers
    {
        public static readonly XmlDocument SampleLocalization;
        static Helpers()
        {
            SampleLocalization = new XmlDocument();
            var assemblyFileName = new FileInfo(typeof(Helpers).Assembly.Location);
            SampleLocalization.Load(assemblyFileName.Directory.GetFiles(@"SampleLoc.xml").First().OpenRead());
        }
    }

    [TestFixture]
    public class LocalizeTests
    {
        [Test]
        public void LocalizesSimpleNodeCorrectly()
        {
            LocalizationService service = new LocalizationService(Helpers.SampleLocalization);
            service.Culture = CultureInfo.GetCultureInfo("bg-BG");

            // note: the expected string "ОК" is actually written using the cyrillic О and К;
            // comparisons with the english OK will fail
            Assert.AreEqual("ОК", service.Localize("NodeA1.OK"));
        }

        [Test]
        public void LocalizesKeysWithoutCaseSensitivity()
        {
            LocalizationService service = new LocalizationService(Helpers.SampleLocalization);
            service.Culture = CultureInfo.GetCultureInfo("bg-BG");

            // note: the expected string "ОК" is actually written using the cyrillic О and К;
            // comparisons with the english OK will fail
            Assert.AreEqual("ОК", service.Localize("NodeA1.OK"));
            Assert.AreEqual("ОК", service.Localize("nodea1.ok"));
        }

        [Test]
        public void LocalizesComplexNodeCorrectly1()
        {
            LocalizationService service = new LocalizationService(Helpers.SampleLocalization);
            service.Culture = CultureInfo.GetCultureInfo("de-DE");

            Assert.AreEqual("Abbrechen", service.Localize("NodeB1.NodeB2.Cancel"));
        }

        [Test]
        public void LocalizesComplexNodeCorrectly2()
        {
            LocalizationService service = new LocalizationService(Helpers.SampleLocalization);
            service.Culture = CultureInfo.GetCultureInfo("bg-BG");

            Assert.AreEqual("Приложи", service.Localize("NodeB1.NodeB3.NodeB4.Apply"));
        }

        [Test]
        public void ReturnsLocalizationKeyIfNoLocalizationPresent1()
        {
            LocalizationService service = new LocalizationService(Helpers.SampleLocalization);
            service.Culture = CultureInfo.GetCultureInfo("bg-BG");

            Assert.AreEqual("[bg-bg]+nodeb2", service.Localize("NodeB2"));

        }

        [Test]
        public void ReturnsLocalizationKeyIfNoLocalizationPresent2()
        {
            LocalizationService service = new LocalizationService(Helpers.SampleLocalization);
            service.Culture = CultureInfo.GetCultureInfo("de-DE");

            Assert.AreEqual("[de-de]+nodeb5.close", service.Localize("NodeB5.Close"));
        }

        [Test]
        public void LocalizesWithCultureOverride()
        {
            LocalizationService service = new LocalizationService(Helpers.SampleLocalization);
            Assert.AreEqual("Abbrechen", service.Localize("NodeB1.NodeB2.Cancel", CultureInfo.GetCultureInfo("de-DE")));
        }
    }

    [TestFixture]
    public class CulturePropertyTests
    {
        CultureInfo oldCulture;
        CultureInfo testCulture;

        [OneTimeSetUp]
        public void Init()
        {
            oldCulture = Thread.CurrentThread.CurrentCulture;
            testCulture = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(ci => ci != oldCulture).First();
            Thread.CurrentThread.CurrentCulture = testCulture;
        }

        [Test]
        public void SetsToCurrentCultureByDefault()
        {
            LocalizationService service = new LocalizationService(Helpers.SampleLocalization);
            Assert.AreEqual(testCulture, service.Culture);
        }

        [Test]
        public void ThrowsIfCultureIsInvariant()
        {
            LocalizationService service = new LocalizationService(Helpers.SampleLocalization);
            Assert.Throws<InvalidOperationException>(() => service.Culture = CultureInfo.InvariantCulture);
        }

        [Test]
        public void ThrowsIfCultureIsNull()
        {
            LocalizationService service = new LocalizationService(Helpers.SampleLocalization);
            Assert.Throws<InvalidOperationException>(() => service.Culture = null);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            Thread.CurrentThread.CurrentCulture = oldCulture;
        }
    }
}
