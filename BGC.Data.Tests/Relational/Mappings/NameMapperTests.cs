using BGC.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Data.Relational.Mappings.NameMapperTests
{
    public class CopyDataTests : TestFixtureBase
    {
        private readonly NameMapper _mapper = new NameMapper();

        [Test]
        public void CopiesDataToDto()
        {
            ComposerName name = new ComposerName("Johnny English", "en-GB");
            NameRelationalDto dto = _mapper.CopyData(name, new NameRelationalDto());

            Assert.AreEqual(name.FullName, dto.FullName);
            Assert.AreEqual(name.Language.Name, dto.Language);
        }
    }
}
