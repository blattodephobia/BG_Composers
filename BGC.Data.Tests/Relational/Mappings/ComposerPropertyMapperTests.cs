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

namespace BGC.Data.Relational.Mappings.ComposerPropertyMapperTests
{
    public class CopyDataTests : TestFixtureBase
    {
        private readonly ComposerPropertyMapper _mapper = new ComposerPropertyMapper();
        
        [Test]
        public void CopiesDataCorrectly()
        {
            var composer = new Composer();
            composer.Id = new Guid(7, 1, 5, new byte[8]);
            composer.DateAdded = new DateTime(2018, 1, 1);
            composer.DateOfBirth = new DateTime(1970, 2, 2);
            composer.DateOfDeath = new DateTime(2070, 2, 2);
            composer.Order = 4;

            var dto = new ComposerRelationalDto();

            _mapper.CopyData(composer, dto);

            Assert.AreEqual(composer.Id, dto.Id);
            Assert.AreEqual(composer.DateAdded, dto.DateAdded);
            Assert.AreEqual(composer.DateOfBirth, dto.DateOfBirth);
            Assert.AreEqual(composer.DateOfDeath, dto.DateOfDeath);
            Assert.AreEqual(composer.Order, dto.Order);
        }

        [Test]
        public void CopiesNullDateTimeAddedAsSpecialDateTimeValue()
        {
            var composer = new Composer();
            composer.DateAdded = null;

            var dto = new ComposerRelationalDto();

            _mapper.CopyData(composer, dto);

            Assert.AreEqual(DateTime.MinValue, dto.DateAdded);
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
                DateAdded = new DateTime(2018, 1, 1),
                DateOfBirth = new DateTime(1970, 2, 2),
                DateOfDeath = new DateTime(2070, 2, 2),
                Order = 3,
            };

            Composer entity = _mapper.CopyData(dto, new Composer());

            Assert.AreEqual(dto.Id, entity.Id);
            Assert.AreEqual(dto.DateAdded, entity.DateAdded);
            Assert.AreEqual(dto.DateOfBirth, entity.DateOfBirth);
            Assert.AreEqual(dto.DateOfDeath, entity.DateOfDeath);
            Assert.AreEqual(dto.Order, entity.Order);
        }
    }
}
