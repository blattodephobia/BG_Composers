using System;
using NUnit.Framework;
using System.Xml;
using System.IO;
using System.Linq;
using System.Threading;
using System.Globalization;
using TestUtils;

namespace BGC.Services.Tests
{
    [TestFixture]
    public class LocalizeTests
    {
        [Test]
        public void LocalizesSimpleNodeCorrectly()
        {
            LocalizationService service = new LocalizationService(MockUtilities.SampleLocalization);
            service.DefaultCulture = CultureInfo.GetCultureInfo("bg-BG");

            // note: the expected string "ОК" is actually written using the cyrillic О and К;
            // comparisons with the english OK will fail
            Assert.AreEqual("ОК", service.Localize("NodeA1.OK"));
        }

        [Test]
        public void LocalizesKeysWithoutCaseSensitivity()
        {
            LocalizationService service = new LocalizationService(MockUtilities.SampleLocalization);
            service.DefaultCulture = CultureInfo.GetCultureInfo("bg-BG");

            // note: the expected string "ОК" is actually written using the cyrillic О and К;
            // comparisons with the english OK will fail
            Assert.AreEqual("ОК", service.Localize("NodeA1.OK"));
            Assert.AreEqual("ОК", service.Localize("nodea1.ok"));
        }

        [Test]
        public void LocalizesComplexNodeCorrectly1()
        {
            LocalizationService service = new LocalizationService(MockUtilities.SampleLocalization);
            service.DefaultCulture = CultureInfo.GetCultureInfo("de-DE");

            Assert.AreEqual("Abbrechen", service.Localize("NodeB1.NodeB2.Cancel"));
        }

        [Test]
        public void LocalizesComplexNodeCorrectly2()
        {
            LocalizationService service = new LocalizationService(MockUtilities.SampleLocalization);
            service.DefaultCulture = CultureInfo.GetCultureInfo("bg-BG");

            Assert.AreEqual("Приложи", service.Localize("NodeB1.NodeB3.NodeB4.Apply"));
        }

        [Test]
        public void ReturnsLocalizationKeyIfNoLocalizationPresent1()
        {
            LocalizationService service = new LocalizationService(MockUtilities.SampleLocalization);
            service.DefaultCulture = CultureInfo.GetCultureInfo("bg-BG");

            Assert.AreEqual("[bg-bg]+nodeb2", service.Localize("NodeB2"));

        }

        [Test]
        public void ReturnsLocalizationKeyIfNoLocalizationPresent2()
        {
            LocalizationService service = new LocalizationService(MockUtilities.SampleLocalization);
            service.DefaultCulture = CultureInfo.GetCultureInfo("de-DE");

            Assert.AreEqual("[de-de]+nodeb5.close", service.Localize("NodeB5.Close"));
        }

        [Test]
        public void LocalizesWithCultureOverride()
        {
            LocalizationService service = new LocalizationService(MockUtilities.SampleLocalization);
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
            LocalizationService service = new LocalizationService(MockUtilities.SampleLocalization);
            Assert.AreEqual(testCulture, service.DefaultCulture);
        }

        [Test]
        public void ThrowsIfCultureIsInvariant()
        {
            LocalizationService service = new LocalizationService(MockUtilities.SampleLocalization);
            Assert.Throws<InvalidOperationException>(() => service.DefaultCulture = CultureInfo.InvariantCulture);
        }

        [Test]
        public void ThrowsIfCultureIsNull()
        {
            LocalizationService service = new LocalizationService(MockUtilities.SampleLocalization);
            Assert.Throws<InvalidOperationException>(() => service.DefaultCulture = null);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            Thread.CurrentThread.CurrentCulture = oldCulture;
        }
    }

    [TestFixture]
    public class AlphabetTests
    {
        /// <summary>
        /// This test makes sure that modifications of a char array returned by <see cref="LocalizationService.GetAlphabet(CultureInfo)"/> will not
        /// be present in subsequent char arrays returned by the alphabet.
        /// </summary>
        [Test]
        public void TestCopiesAreOfAlphabetsAreReturned()
        {
            CultureInfo testCulture = CultureInfo.GetCultureInfo("en-US");
            LocalizationService svc = new LocalizationService(MockUtilities.SampleLocalization);
            char[] alphabet = svc.GetAlphabet(culture: testCulture);
            string referenceAlphabet = new string(alphabet);
            for (int i = 0; i < alphabet.Length; i++)
            {
                alphabet[i] = '\0'; // 
            }
            
            Assert.AreEqual(referenceAlphabet, new string(svc.GetAlphabet(culture: testCulture)));
        }
    }
}
