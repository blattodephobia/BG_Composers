using BGC.Core;
using BGC.Data.Relational;
using BGC.Data.Relational.Mappings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Data.Relational.Mappings.ComposerMapperTests
{
    public class CopyDataTests : TestFixtureBase
    {
        private readonly ComposerMapper _mapper = new ComposerMapper();
        
        [Test]
        public void CopiesDataCorrectly()
        {
            var composer = new Composer();
            composer.Id = new Guid(7, 1, 5, new byte[8]);
            composer.DateOfBirth = new DateTime(1970, 2, 2);
            composer.DateOfDeath = new DateTime(2070, 2, 2);
            composer.Order = 4;

            var dto = new ComposerRelationalDto();

            _mapper.CopyData(composer, dto);

            Assert.AreEqual(dto.Id, composer.Id);
            Assert.AreEqual(dto.DateOfBirth, composer.DateOfBirth);
            Assert.AreEqual(dto.DateOfDeath, composer.DateOfDeath);
            Assert.AreEqual(dto.Order, composer.Order);
        }

        [Test]
        public void ThrowsExceptionIfNullSource()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.CopyData(null, new Composer()));
        }

        [Test]
        public void CreatesCorrectEntity()
        {
            var dto = new ComposerRelationalDto()
            {
                Id = new Guid(5, 1, 0, new byte[8]),
                DateOfBirth = new DateTime(1970, 2, 2),
                DateOfDeath = new DateTime(2070, 2, 2),
                Order = 3,
            };

            Composer entity = _mapper.CopyData(dto, new Composer());

            Assert.AreEqual(dto.Id, entity.Id);
            Assert.AreEqual(dto.DateOfBirth, entity.DateOfBirth);
            Assert.AreEqual(dto.DateOfDeath, entity.DateOfDeath);
            Assert.AreEqual(dto.Order, entity.Order);
        }
    }
}
