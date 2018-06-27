using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Data.Attributes.IdentityAttributeTests
{
    public class CtorTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullPropertyName()
        {
            Assert.Throws<ArgumentNullException>(() => new IdentityAttribute(null));
        }

        [Test]
        public void ThrowsExceptionIfEmptyPropertyName()
        {
            Assert.Throws<ArgumentNullException>(() => new IdentityAttribute(""));
        }

        [Test]
        public void ThrowsExceptionIfWhitespacePropertyName()
        {
            Assert.Throws<ArgumentNullException>(() => new IdentityAttribute("   "));
        }
    }
}
