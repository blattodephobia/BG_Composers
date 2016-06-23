using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace BGC.Core.Tests.Models
{
	[TestClass]
	public class ComposerNameTests
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

		[TestClass]
		public class NameTests
		{
			[TestMethod]
			public void FirstAndLastNameDependencies()
			{
				ComposerName name = new ComposerName();
				name.FirstName = "First";
				Assert.AreEqual("First", name.FullName);

				name.LastName = "Last";
				Assert.AreEqual("First Last", name.FullName);

				name.FullName = "First1 Middle1 Last1";
				Assert.AreEqual("First1", name.FirstName);
				Assert.AreEqual("Last1", name.LastName);
			}

            [TestMethod]
            public void SingleNameMapsToLastNameOnly()
            {
                ComposerName name = new ComposerName("Last");
                Assert.AreEqual("Last", name.LastName);
                Assert.IsTrue(string.IsNullOrEmpty(name.FirstName));
            }
		}

        [TestClass]
        public class LocalizationCultureNameTests
        {
            [TestMethod]
            public void SetsInternalPropertyCorrectly()
            {
                ComposerNameProxy name = new ComposerNameProxy();
                name.Language = CultureInfo.GetCultureInfo("en-US");
                Assert.AreEqual("en-US", name.LocalizationCultureNameProxy);
            }

            [TestMethod]
            public void SetsPublicPropertyCorrectly()
            {
                ComposerNameProxy name = new ComposerNameProxy();
                name.LocalizationCultureNameProxy = "en-US";
                Assert.AreEqual("en-US", name.Language.Name);
            }
        }

        [TestClass]
        public class GetEasternOrderFullNameTests
        {
            [TestMethod]
            public void ReturnsCorrectNameWithTwoNamesOnly1()
            {
                ComposerName name = new ComposerName("First Last");
                Assert.AreEqual("Last, First", name.GetEasternOrderFullName());
            }

            [TestMethod]
            public void ReturnsCorrectNameWithTwoNamesOnly2()
            {
                ComposerName name = new ComposerName("First Last");
                name.LastName = "Last1";
                Assert.AreEqual("Last1, First", name.GetEasternOrderFullName());
            }

            [TestMethod]
            public void ReturnsCorrectNameWithThreeNames1()
            {
                ComposerName name = new ComposerName("First Middle Last");
                Assert.AreEqual("Last, First Middle", name.GetEasternOrderFullName());
            }

            [TestMethod]
            public void ReturnsCorrectNameWithThreeNames2()
            {
                ComposerName name = new ComposerName("First Middle Last");
                name.FirstName = "FIRST1";
                Assert.AreEqual("Last, FIRST1 Middle", name.GetEasternOrderFullName());
            }

            [TestMethod]
            public void ReturnsCorrectNameWithSingleNameOnly()
            {
                ComposerName name = new ComposerName("Last");
                Assert.AreEqual("Last", name.GetEasternOrderFullName());
            }
        }
	}
}
