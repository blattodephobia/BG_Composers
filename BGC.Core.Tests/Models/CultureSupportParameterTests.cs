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
    public class CultureSupportParameterTests
    {
        [TestClass]
        public class SupportedCulturesTests
        {
            [TestMethod]
            public void AcceptsNull()
            {
                CultureSupportParameter s = new CultureSupportParameter();
                s.SupportedCultures = null;
                Assert.IsNull(s.StringValue);
            }

            [TestMethod]
            public void IsPopulatedFromStringCorrectly1()
            {
                CultureSupportParameter s = new CultureSupportParameter();
                s.StringValue = "en-US, bg-BG, de-DE";
                Assert.AreEqual("en-US", s.SupportedCultures.ElementAt(0).Name);
                Assert.AreEqual("bg-BG", s.SupportedCultures.ElementAt(1).Name);
                Assert.AreEqual("de-DE", s.SupportedCultures.ElementAt(2).Name);
            }

            [TestMethod]
            public void IsPopulatedFromStringCorrectly2()
            {
                CultureSupportParameter s = new CultureSupportParameter();
                s.StringValue = "en-US";
                Assert.AreEqual("en-US", s.SupportedCultures.ElementAt(0).Name);
            }
        }

        [TestClass]
        public class StringValue
        {
            [TestMethod]
            public void AcceptsNull()
            {
                CultureSupportParameter s = new CultureSupportParameter();
                s.StringValue = null;
                Assert.AreEqual(null, s.SupportedCultures);
            }

            [TestMethod]
            public void IsSetFromSupportedCulturesProperty1()
            {
                CultureSupportParameter s = new CultureSupportParameter();
                s.SupportedCultures = new[] { CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("tr-TR") };
                Assert.AreEqual("en-US, tr-TR", s.StringValue);
            }

            [TestMethod]
            public void IsSetFromSupportedCulturesProperty2()
            {
                CultureSupportParameter s = new CultureSupportParameter();
                s.SupportedCultures = new[] { CultureInfo.GetCultureInfo("en-US") };
                Assert.AreEqual("en-US", s.StringValue);
            }

            [TestMethod]
            [ExpectedException(typeof(CultureNotFoundException))]
            public void FormatTest()
            {
                CultureSupportParameter s = new CultureSupportParameter(); ;
                s.StringValue = "df-QQ";
            }
        }
    }
}
