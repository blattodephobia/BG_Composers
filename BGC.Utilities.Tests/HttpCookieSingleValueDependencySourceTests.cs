using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BGC.Utilities.Tests
{
    [TestFixture]
    public class SetValueTests
    {
        [Test]
        public void SetsValueToCookie()
        {
            HttpCookie cookie = new HttpCookie("locale");
            SingleValueDependencySource<CultureInfo> localeValue = new HttpCookieSingleValueDependencySource<CultureInfo>(cookie, "key", new CultureInfoConverter());

            var value = new CultureInfo("de-DE");
            localeValue.SetValue(value);
            Assert.AreEqual("de-DE", cookie["key"]);
            Assert.AreSame(value, localeValue.GetEffectiveValue());

            value = new CultureInfo("en-US");
            localeValue.SetValue(value);
            Assert.AreEqual("en-US", cookie["key"]);
            Assert.AreSame(value, localeValue.GetEffectiveValue());
        }
    }

    [TestFixture]
    public class HasValueTests
    {
        [Test]
        public void HasNoValueByDefault()
        {
            HttpCookie cookie = new HttpCookie("locale");
            SingleValueDependencySource<CultureInfo> localeValue = new HttpCookieSingleValueDependencySource<CultureInfo>(cookie, "key", new CultureInfoConverter());

            Assert.IsFalse(localeValue.HasValue);
        }

        [Test]
        public void HasValueWhenSet()
        {
            HttpCookie cookie = new HttpCookie("locale");
            SingleValueDependencySource<CultureInfo> localeValue = new HttpCookieSingleValueDependencySource<CultureInfo>(cookie, "key", new CultureInfoConverter());

            var value = new CultureInfo("de-DE");
            localeValue.SetValue(value);
            Assert.IsTrue(localeValue.HasValue);
        }

        [Test]
        public void HasNoValueWhenUnset()
        {
            HttpCookie cookie = new HttpCookie("locale");
            SingleValueDependencySource<CultureInfo> localeValue = new HttpCookieSingleValueDependencySource<CultureInfo>(cookie, "key", new CultureInfoConverter());

            var value = new CultureInfo("de-DE");
            localeValue.SetValue(value);
            localeValue.UnsetValue();
            Assert.IsFalse(localeValue.HasValue);
        }

        [Test]
        public void HasValueAfterExternalSet()
        {
            HttpCookie cookie = new HttpCookie("locale");
            SingleValueDependencySource<CultureInfo> localeValue = new HttpCookieSingleValueDependencySource<CultureInfo>(cookie, "key", new CultureInfoConverter());
            
            cookie["key"] = "en-US";
            Assert.IsTrue(localeValue.HasValue);
        }

        [Test]
        public void HasNoValueAfterExternalUnSet()
        {
            HttpCookie cookie = new HttpCookie("locale");
            SingleValueDependencySource<CultureInfo> localeValue = new HttpCookieSingleValueDependencySource<CultureInfo>(cookie, "key", new CultureInfoConverter());
            
            cookie["key"] = "en-US";
            Assert.IsTrue(localeValue.HasValue);

            cookie["key"] = null;
            Assert.IsFalse(localeValue.HasValue);
        }
    }

    [TestFixture]
    public class GetEffectiveValueTests
    {
        [Test]
        public void Initial()
        {
            HttpCookie cookie = new HttpCookie("locale");
            SingleValueDependencySource<CultureInfo> localeValue = new HttpCookieSingleValueDependencySource<CultureInfo>(cookie, "key", new CultureInfoConverter());
            
            Assert.IsNull(localeValue.GetEffectiveValue());
        }

        [Test]
        public void GetsValueAfterExternalSet()
        {
            HttpCookie cookie = new HttpCookie("locale");
            SingleValueDependencySource<CultureInfo> localeValue = new HttpCookieSingleValueDependencySource<CultureInfo>(cookie, "key", new CultureInfoConverter());

            cookie["key"] = "de-DE";
            Assert.AreEqual("de-DE", localeValue.GetEffectiveValue().Name);
        }

        [Test]
        public void ReturnsNullAfterExternalUnSet()
        {
            HttpCookie cookie = new HttpCookie("locale");
            SingleValueDependencySource<CultureInfo> localeValue = new HttpCookieSingleValueDependencySource<CultureInfo>(cookie, "key", new CultureInfoConverter());

            cookie["key"] = "de-DE";
            Assert.AreEqual("de-DE", localeValue.GetEffectiveValue().Name);

            cookie["key"] = null;
            Assert.IsNull(localeValue.GetEffectiveValue());
        }
    }
}
