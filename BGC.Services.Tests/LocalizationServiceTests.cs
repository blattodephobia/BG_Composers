using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.IO;
using System.Linq;
using System.Threading;
using System.Globalization;

namespace BGC.Services.Tests
{
    [TestClass]
    public class LocalizationServiceTests
    {
        static XmlDocument sampleLocalization;
        static LocalizationServiceTests()
        {
            sampleLocalization = new XmlDocument();
            sampleLocalization.Load(File.OpenRead(@"SampleLoc.xml"));
        }

        [TestClass]
        public class LocalizeTests
        {
            [TestMethod]
            public void LocalizesSimpleNodeCorrectly()
            {
                LocalizationService service = new LocalizationService(sampleLocalization);
                service.Culture = CultureInfo.GetCultureInfo("bg-BG");

                // note: the expected string "ОК" is actually written using the cyrillic О and К;
                // comparisons with the english OK will fail
                Assert.AreEqual("ОК", service.Localize("NodeA1.OK"));
            }

            [TestMethod]
            public void LocalizesKeysWithoutCaseSensitivity()
            {
                LocalizationService service = new LocalizationService(sampleLocalization);
                service.Culture = CultureInfo.GetCultureInfo("bg-BG");

                // note: the expected string "ОК" is actually written using the cyrillic О and К;
                // comparisons with the english OK will fail
                Assert.AreEqual("ОК", service.Localize("NodeA1.OK"));
                Assert.AreEqual("ОК", service.Localize("nodea1.ok"));
            }

            [TestMethod]
            public void LocalizesComplexNodeCorrectly1()
            {
                LocalizationService service = new LocalizationService(sampleLocalization);
                service.Culture = CultureInfo.GetCultureInfo("de-DE");

                Assert.AreEqual("Abbrechen", service.Localize("NodeB1.NodeB2.Cancel"));
            }

            [TestMethod]
            public void LocalizesComplexNodeCorrectly2()
            {
                LocalizationService service = new LocalizationService(sampleLocalization);
                service.Culture = CultureInfo.GetCultureInfo("bg-BG");

                Assert.AreEqual("Приложи", service.Localize("NodeB1.NodeB3.NodeB4.Apply"));
            }

            [TestMethod]
            public void ReturnsLocalizationKeyIfNoLocalizationPresent1()
            {
                LocalizationService service = new LocalizationService(sampleLocalization);
                service.Culture = CultureInfo.GetCultureInfo("bg-BG");

                Assert.AreEqual("NodeB2", service.Localize("NodeB2"));

            }

            [TestMethod]
            public void ReturnsLocalizationKeyIfNoLocalizationPresent2()
            {
                LocalizationService service = new LocalizationService(sampleLocalization);
                service.Culture = CultureInfo.GetCultureInfo("de-DE");

                Assert.AreEqual("NodeB5.Close", service.Localize("NodeB5.Close"));
            }
        }

        [TestClass]
        public class CulturePropertyTests
        {
            CultureInfo oldCulture;
            CultureInfo testCulture;

            [TestInitialize]
            public void Init()
            {
                oldCulture = Thread.CurrentThread.CurrentCulture;
                testCulture = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(ci => ci != oldCulture).First();
                Thread.CurrentThread.CurrentCulture = testCulture;
            }

            [TestMethod]
            public void SetsToCurrentCultureByDefault()
            {
                LocalizationService service = new LocalizationService(sampleLocalization);
                Assert.AreEqual(testCulture, service.Culture);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsIfCultureIsInvariant()
            {
                LocalizationService service = new LocalizationService(sampleLocalization);
                service.Culture = CultureInfo.InvariantCulture;
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsIfCultureIsNull()
            {
                LocalizationService service = new LocalizationService(sampleLocalization);
                service.Culture = null;
            }

            [TestCleanup]
            public void Cleanup()
            {
                Thread.CurrentThread.CurrentCulture = oldCulture;
            }
        }
    }
}
