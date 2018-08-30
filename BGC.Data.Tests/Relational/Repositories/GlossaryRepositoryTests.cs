using BGC.Core;
using BGC.Core.Models;
using BGC.Data.Relational.Mappings;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;
using static TestUtils.MockUtilities;

namespace BGC.Data.Relational.Repositories.GlossaryRepositoryTests
{
    public class FindTests : TestFixtureBase
    {
        private readonly GlossaryRepository _repo;
        private readonly List<GlossaryEntryRelationalDto> _storage;

        private DbSet GetBackingStore(Type t)
        {
            if (t == typeof(GlossaryEntryRelationalDto))
            {
                return GetMockDbSet(_storage).Object;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override void BeforeEachTest()
        {
            base.BeforeEachTest();
            _storage.Clear();
        }

        public FindTests()
        {
            _storage = new List<GlossaryEntryRelationalDto>();
            Mock<DbContext> mockCtx = GetMockDbContext();
            mockCtx.Setup(c => c.Set<GlossaryEntryRelationalDto>()).Returns(GetMockDbSet(_storage).Object);
            _repo = new GlossaryRepository(new GlossaryEntryTypeMapper(new GlossaryEntryPropertyMapper(), new MockDtoFactory()), mockCtx.Object);
        }
        
        [Test]
        public void FindsCorrectDefinition()
        {
            IGlossaryEntryDto target = new GlossaryEntryRelationalDto();
            _storage.Add(target as GlossaryEntryRelationalDto);

            GlossaryEntry def = _repo.Find(dto => dto.Id == target.Id).First();

            Assert.AreEqual(target.Id, def.Id);
        }

        [Test]
        public void ThrowsExceptionIfNullSelector()
        {
            Assert.Throws<ArgumentNullException>(() => _repo.Find(selector: null));
        }
    }
}
