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
    public class CultureSupportSettingTests
    {
        [TestClass]
        public class SupportedCulturesTests
        {
            [TestMethod]
            public void AcceptsNull()
            {
                CultureSupportSetting s = new CultureSupportSetting();
                s.SupportedCultures = null;
                Assert.IsNull(s.StringValue);
            }

            [TestMethod]
            public void IsPopulatedFromStringCorrectly1()
            {
                CultureSupportSetting s = new CultureSupportSetting();
                s.StringValue = "en-US, bg-BG, de-DE";
                Assert.AreEqual("en-US", s.SupportedCultures.ElementAt(0).Name);
                Assert.AreEqual("bg-BG", s.SupportedCultures.ElementAt(1).Name);
                Assert.AreEqual("de-DE", s.SupportedCultures.ElementAt(2).Name);
            }

            [TestMethod]
            public void IsPopulatedFromStringCorrectly2()
            {
                CultureSupportSetting s = new CultureSupportSetting();
                s.StringValue = "en-US";
                Assert.AreEqual("en-US", s.SupportedCultures.ElementAt(0).Name);
            }
        }

        [TestClass]
        public class StringValue
        {
            [TestMethod]
            public void IsSetFromSupportedCulturesProperty1()
            {
                CultureSupportSetting s = new CultureSupportSetting();
                s.SupportedCultures = new[] { CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("tr-TR") };
                Assert.AreEqual("en-US, tr-TR", s.StringValue);
            }

            [TestMethod]
            public void IsSetFromSupportedCulturesProperty2()
            {
                CultureSupportSetting s = new CultureSupportSetting();
                s.SupportedCultures = new[] { CultureInfo.GetCultureInfo("en-US") };
                Assert.AreEqual("en-US", s.StringValue);
            }

            [TestMethod]
            [ExpectedException(typeof(CultureNotFoundException))]
            public void FormatTest()
            {
                CultureSupportSetting s = new CultureSupportSetting(); ;
                s.StringValue = "df-QQ";
            }
        }

        [TestClass]
        public class CtorTests
        {
            [TestMethod]
            public void ConstructsCorrectlyWithValidInput()
            {
                CultureSupportSetting param = new CultureSupportSetting("bg-BG, de-DE");
                Assert.AreEqual("bg-BG", param.SupportedCultures.ElementAt(0).Name);
                Assert.AreEqual("de-DE", param.SupportedCultures.ElementAt(1).Name);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsExceptionOnNullValue()
            {
                CultureSupportSetting param = new CultureSupportSetting(null);
            }
        }
    }
}
