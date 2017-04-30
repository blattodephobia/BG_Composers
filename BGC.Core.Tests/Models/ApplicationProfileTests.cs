using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models
{
    [TestFixture]
    public class ApplicationProfileTests
    {
        [Test]
        public void ThrowsExceptionIfSealed()
        {
            ApplicationProfile profile = new ApplicationProfile();
            profile.LocaleCookieName = "abc";
            Assert.AreEqual("abc", profile.LocaleCookieName);
            profile.Seal();
            Assert.Throws<InvalidOperationException>(() => profile.LocaleCookieName = "def");
        }

        [Test]
        public void SealsCorrectly()
        {
            ApplicationProfile profile = new ApplicationProfile();

            Assert.IsFalse(profile.IsSealed);
            profile.Seal();
            Assert.IsTrue(profile.IsSealed);
        }
    }
}
