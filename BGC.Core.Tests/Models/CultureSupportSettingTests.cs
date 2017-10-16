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
            MultiCultureInfoSetting s = new MultiCultureInfoSetting();
            s.Cultures = null;
            Assert.IsNull(s.StringValue);
        }

        [Test]
        public void IsPopulatedFromStringCorrectly1()
        {
            MultiCultureInfoSetting s = new MultiCultureInfoSetting();
            s.StringValue = "en-US, bg-BG, de-DE";
            Assert.AreEqual("en-US", s.Cultures.ElementAt(0).Name);
            Assert.AreEqual("bg-BG", s.Cultures.ElementAt(1).Name);
            Assert.AreEqual("de-DE", s.Cultures.ElementAt(2).Name);
        }

        [Test]
        public void IsPopulatedFromStringCorrectly2()
        {
            MultiCultureInfoSetting s = new MultiCultureInfoSetting();
            s.StringValue = "en-US";
            Assert.AreEqual("en-US", s.Cultures.ElementAt(0).Name);
        }
    }

    [TestFixture]
    public class StringValue
    {
        [Test]
        public void IsSetFromSupportedCulturesProperty1()
        {
            MultiCultureInfoSetting s = new MultiCultureInfoSetting();
            s.Cultures = new[] { CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("tr-TR") };
            Assert.AreEqual("en-US, tr-TR", s.StringValue);
        }

        [Test]
        public void IsSetFromSupportedCulturesProperty2()
        {
            MultiCultureInfoSetting s = new MultiCultureInfoSetting();
            s.Cultures = new[] { CultureInfo.GetCultureInfo("en-US") };
            Assert.AreEqual("en-US", s.StringValue);
        }

        [Test]
        public void FormatTest()
        {
            MultiCultureInfoSetting s = new MultiCultureInfoSetting(); ;
            Assert.Throws<CultureNotFoundException>(() => s.StringValue = "dfQQ");
        }
    }

    [TestFixture]
    public class CtorTests
    {
        [Test]
        public void ConstructsCorrectlyWithValidInput()
        {
            MultiCultureInfoSetting param = new MultiCultureInfoSetting("bg-BG, de-DE");
            Assert.AreEqual("bg-BG", param.Cultures.ElementAt(0).Name);
            Assert.AreEqual("de-DE", param.Cultures.ElementAt(1).Name);
        }

        [Test]
        public void ThrowsExceptionOnNullValue()
        {
            Assert.Throws<ArgumentNullException>(() => new MultiCultureInfoSetting(null));
        }
    }
}
