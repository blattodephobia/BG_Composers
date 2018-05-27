using BGC.Core;
using BGC.Data.Relational;
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

namespace BGC.Data.EntityFrameworkRepositoryTests
{
    public class CtorTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullMapper()
        {
            Assert.Throws<ArgumentNullException>(() => new EntityFrameworkRepository<Guid, Composer, ComposerRelationalDto>(null, new Mock<IDbSet<ComposerRelationalDto>>().Object));
        }

        [Test]
        public void ThrowsExceptionIfNullDbSet()
        {
            Assert.Throws<ArgumentNullException>(() => new EntityFrameworkRepository<Guid, Composer, ComposerRelationalDto>(new ComposerMapper(), null));
        }
    }

    public class AddOrUpdateTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullEntity()
        {
            var repo = new EntityFrameworkRepository<Guid, Composer, ComposerRelationalDto>(new ComposerMapper(), GetMockDbSet<ComposerRelationalDto>().Object);
            Assert.Throws<ArgumentNullException>(() => repo.AddOrUpdate(null));
        }

        [Test]
        public void AddsEntityIfNotPresent()
        {
            var db = new List<ComposerRelationalDto>();
            Mock<IDbSet<ComposerRelationalDto>> dbSet = GetMockDbSet(db);
            dbSet.Setup(r => r.Find(It.Is((object[] keys) => keys.Length == 1))).Returns((object[] keys) => db.FirstOrDefault(dto => dto.Id == (Guid)keys[0]));
            
            var repo = new EntityFrameworkRepository<Guid, Composer, ComposerRelationalDto>(new ComposerMapper(), dbSet.Object);


            Guid id = new Guid(7, 7, 7, new byte[8]);
            repo.AddOrUpdate(new Composer() { Id = id });

            Assert.AreEqual(1, db.Count, "Entity wasn't added to the backing store.");
            Assert.AreEqual(id, db[0].Id);
        }

        [Test]
        public void UpdatesEntityIfPresent()
        {
            Guid id = new Guid(7, 7, 7, new byte[8]);
            var db = new List<ComposerRelationalDto>() { new ComposerRelationalDto() { Id = id } };
            Mock<IDbSet<ComposerRelationalDto>> dbSet = GetMockDbSet(db);
            dbSet.Setup(r => r.Find(It.Is((object[] keys) => keys.Length == 1))).Returns((object[] keys) => db.FirstOrDefault(dto => dto.Id == (Guid)keys[0]));
            var repo = new EntityFrameworkRepository<Guid, Composer, ComposerRelationalDto>(new ComposerMapper(), dbSet.Object);

            var composer = new Composer()
            {
                Id = id,
                DateOfBirth = new DateTime(1980, 1, 3)
            };
            repo.AddOrUpdate(composer);

            Assert.AreEqual(composer.DateOfBirth, db[0].DateOfBirth);
        }
    }

    public class DeleteTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullEntity()
        {
            var repo = new EntityFrameworkRepository<Guid, Composer, ComposerRelationalDto>(new ComposerMapper(), GetMockDbSet<ComposerRelationalDto>().Object);

            Assert.Throws<ArgumentNullException>(() => repo.Delete(null));
        }

        [Test]
        public void DeletesEntities()
        {
            Guid id1 = new Guid(7, 7, 7, new byte[8]);
            Guid id2 = new Guid(8, 8, 8, new byte[8]);
            var db = new List<ComposerRelationalDto>() { new ComposerRelationalDto() { Id = id1 }, new ComposerRelationalDto() { Id = id2 } };
            Mock<IDbSet<ComposerRelationalDto>> mockDbSet = GetMockDbSet(db);
            mockDbSet.Setup(s => s.Find(It.Is((object[] keys) => keys.Length == 1))).Returns((object[] keys) => db.Where(dto => dto.Id == (Guid)keys[0]).FirstOrDefault());
            var repo = new EntityFrameworkRepository<Guid, Composer, ComposerRelationalDto>(new ComposerMapper(), mockDbSet.Object);

            repo.Delete(new Composer() { Id = id1 });

            Assert.IsFalse(db.Any(dto => dto.Id == id1));
        }
    }

    public class FindTests : TestFixtureBase
    {
        [Test]
        public void ReturnsEntityIfPresent()
        {
            Guid id1 = new Guid(7, 7, 7, new byte[8]);
            Guid id2 = new Guid(8, 8, 8, new byte[8]);
            var db = new List<ComposerRelationalDto>() { new ComposerRelationalDto() { Id = id1 }, new ComposerRelationalDto() { Id = id2 } };
            Mock<IDbSet<ComposerRelationalDto>> dbSet = GetMockDbSet(db);
            dbSet.Setup(r => r.Find(It.Is((object[] keys) => keys.Length == 1))).Returns((object[] keys) => db.FirstOrDefault(dto => dto.Id == (Guid)keys[0]));
            var repo = new EntityFrameworkRepository<Guid, Composer, ComposerRelationalDto>(new ComposerMapper(), dbSet.Object);

            Composer entity = repo.Find(id1);

            Assert.IsNotNull(entity);
            Assert.AreEqual(id1, entity.Id);
        }

        [Test]
        public void ReturnsNullIfEntityNotFound()
        {
            Guid id1 = new Guid(7, 7, 7, new byte[8]);
            Guid id2 = new Guid(8, 8, 8, new byte[8]);
            var db = new List<ComposerRelationalDto>() { new ComposerRelationalDto() { Id = id1 }, new ComposerRelationalDto() { Id = id2 } };
            Mock<IDbSet<ComposerRelationalDto>> dbSet = GetMockDbSet(db);
            dbSet.Setup(r => r.Find(It.Is((object[] keys) => keys.Length == 1))).Returns((object[] keys) => db.FirstOrDefault(dto => dto.Id == (Guid)keys[0]));
            var repo = new EntityFrameworkRepository<Guid, Composer, ComposerRelationalDto>(new ComposerMapper(), dbSet.Object);

            Guid id3 = new Guid(9, 9, 9, new byte[8]);
            Composer entity = repo.Find(id3);

            Assert.IsNull(entity);
        }
    }
}
