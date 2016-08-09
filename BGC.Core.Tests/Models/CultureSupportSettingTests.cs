using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models
{
    [TestFixture]
    public class SupportedCulturesTests
    {
        [Test]
        public void AcceptsNull()
        {
            CultureSupportSetting s = new CultureSupportSetting();
            s.SupportedCultures = null;
            Assert.IsNull(s.StringValue);
        }

        [Test]
        public void IsPopulatedFromStringCorrectly1()
        {
            CultureSupportSetting s = new CultureSupportSetting();
            s.StringValue = "en-US, bg-BG, de-DE";
            Assert.AreEqual("en-US", s.SupportedCultures.ElementAt(0).Name);
            Assert.AreEqual("bg-BG", s.SupportedCultures.ElementAt(1).Name);
            Assert.AreEqual("de-DE", s.SupportedCultures.ElementAt(2).Name);
        }

        [Test]
        public void IsPopulatedFromStringCorrectly2()
        {
            CultureSupportSetting s = new CultureSupportSetting();
            s.StringValue = "en-US";
            Assert.AreEqual("en-US", s.SupportedCultures.ElementAt(0).Name);
        }
    }

    [TestFixture]
    public class StringValue
    {
        [Test]
        public void IsSetFromSupportedCulturesProperty1()
        {
            CultureSupportSetting s = new CultureSupportSetting();
            s.SupportedCultures = new[] { CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("tr-TR") };
            Assert.AreEqual("en-US, tr-TR", s.StringValue);
        }

        [Test]
        public void IsSetFromSupportedCulturesProperty2()
        {
            CultureSupportSetting s = new CultureSupportSetting();
            s.SupportedCultures = new[] { CultureInfo.GetCultureInfo("en-US") };
            Assert.AreEqual("en-US", s.StringValue);
        }

        [Test]
        public void FormatTest()
        {
            CultureSupportSetting s = new CultureSupportSetting(); ;
            Assert.Throws<CultureNotFoundException>(() => s.StringValue = "dfQQ");
        }
    }

    [TestFixture]
    public class CtorTests
    {
        [Test]
        public void ConstructsCorrectlyWithValidInput()
        {
            CultureSupportSetting param = new CultureSupportSetting("bg-BG, de-DE");
            Assert.AreEqual("bg-BG", param.SupportedCultures.ElementAt(0).Name);
            Assert.AreEqual("de-DE", param.SupportedCultures.ElementAt(1).Name);
        }

        [Test]
        public void ThrowsExceptionOnNullValue()
        {
            Assert.Throws<ArgumentNullException>(() => new CultureSupportSetting(null));
        }
    }
}
