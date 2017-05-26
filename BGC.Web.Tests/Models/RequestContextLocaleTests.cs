using BGC.Core;
using BGC.Web.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BGC.Web.Tests.Models
{
    [TestFixture]
    public class RequestContextLocaleTests
    {
        [Test]
        public void ChecksAppProfileForValidData()
        {
            Assert.Throws<InvalidOperationException>(() => new RequestContextLocale(new ApplicationProfile(), new HttpCookie("key")));
        }
    }
}
