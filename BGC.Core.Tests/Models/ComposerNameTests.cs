using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Globalization;

namespace BGC.Core.Tests.Models.ComposerNameTests
{
    class ComposerNameProxy : ComposerName
    {
        public string LocalizationCultureNameProxy
        {
            get
            {
                return LanguageInternal;
            }

            set
            {
                this.LanguageInternal = value;
            }
        }
    }

    [TestFixture]
    public class NameTests
    {
        [Test]
        public void FirstAndLastNameDependencies()
        {
            ComposerName name = new ComposerName("ads", new CultureInfo(1033));
            name.FirstName = "First";
            Assert.AreEqual("First", name.FullName);

            name.LastName = "Last";
            Assert.AreEqual("First Last", name.FullName);

            name.FullName = "First1 Middle1 Last1";
            Assert.AreEqual("First1", name.FirstName);
            Assert.AreEqual("Last1", name.LastName);
        }

        [Test]
        public void SingleNameMapsToLastNameOnly()
        {
            ComposerName name = new ComposerName("Last", new CultureInfo(1033));
            Assert.AreEqual("Last", name.LastName);
            Assert.IsTrue(string.IsNullOrEmpty(name.FirstName));
        }

        [Test]
        public void CanRemoveFirstName()
        {
            ComposerName name = new ComposerName("First Last", "en-US");
            name.FirstName = null;
            Assert.AreEqual("Last", name.FullName);
            Assert.IsNull(name.FirstName);
        }
    }

    [TestFixture]
    public class LocalizationCultureNameTests
    {
        [Test]
        public void SetsInternalPropertyCorrectly()
        {
            ComposerNameProxy name = new ComposerNameProxy();
            name.Language = CultureInfo.GetCultureInfo("en-US");
            Assert.AreEqual("en-US", name.LocalizationCultureNameProxy);
        }

        [Test]
        public void SetsPublicPropertyCorrectly()
        {
            ComposerNameProxy name = new ComposerNameProxy();
            name.LocalizationCultureNameProxy = "en-US";
            Assert.AreEqual("en-US", name.Language.Name);
        }
    }

    [TestFixture]
    public class GetEasternOrderFullNameTests
    {
        [Test]
        public void ReturnsCorrectNameWithTwoNamesOnly1()
        {
            ComposerName name = new ComposerName("First Last", "en-US");
            Assert.AreEqual("Last, First", name.GetEasternOrderFullName());
        }

        [Test]
        public void ReturnsCorrectNameWithTwoNamesOnly2()
        {
            ComposerName name = new ComposerName("First Last", "en-US");
            name.LastName = "Last1";
            Assert.AreEqual("Last1, First", name.GetEasternOrderFullName());
        }

        [Test]
        public void ReturnsCorrectNameWithThreeNames1()
        {
            ComposerName name = new ComposerName("First Middle Last", "en-US");
            Assert.AreEqual("Last, First Middle", name.GetEasternOrderFullName());
        }

        [Test]
        public void ReturnsCorrectNameWithThreeNames2()
        {
            ComposerName name = new ComposerName("First Middle Last", "en-US");
            name.FirstName = "FIRST1";
            Assert.AreEqual("Last, FIRST1 Middle", name.GetEasternOrderFullName());
        }

        [Test]
        public void ReturnsCorrectNameWithSingleNameOnly()
        {
            ComposerName name = new ComposerName("Last", "en-US");
            Assert.AreEqual("Last", name.GetEasternOrderFullName());
        }
    }
}
