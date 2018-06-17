using BGC.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Data.Relational.Mappings.DomainBuilderTests
{
    public class CtorTests : TestFixtureBase
    {
        private class DomainBuilderProxy : DomainBuilderBase<ComposerRelationalDto, Composer>
        {
            protected override Composer BuildInternal(ComposerRelationalDto dto)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void ThrowsExceptionIfDtoNull()
        {
            var builder = new DomainBuilderProxy();
            Assert.Throws<ArgumentNullException>(() => builder.Build(null));
        }
    }
}
