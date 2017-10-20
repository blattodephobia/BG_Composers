using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Web.Tests.WebApplicationSettingsTests
{
    [TestFixture]
    public class SealTests
    {
        [Test]
        public void ThrowsExceptionIfSealed()
        {
            WebApplicationSettings profile = new WebApplicationSettings();
            profile.LocaleCookieName = "abc";
            Assert.AreEqual("abc", profile.LocaleCookieName);
            profile.Seal();
            Assert.Throws<InvalidOperationException>(() => profile.LocaleCookieName = "def");
        }

        [Test]
        public void SealsCorrectly()
        {
            WebApplicationSettings profile = new WebApplicationSettings();

            Assert.IsFalse(profile.IsSealed);
            profile.Seal();
            Assert.IsTrue(profile.IsSealed);
        }
    }
}
