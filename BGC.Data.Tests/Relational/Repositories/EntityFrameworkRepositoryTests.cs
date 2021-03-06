﻿using BGC.Core;
using BGC.Data.Relational;
using BGC.Data.Relational.Mappings;
using BGC.Data.Relational.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestUtils;
using static TestUtils.MockUtilities;

namespace BGC.Data.EntityFrameworkRepositoryTests
{
    internal class EFRepoProxy : EntityFrameworkRepository<Guid, Composer, ComposerRelationalDto>
    {
        public EFRepoProxy(BreakdownProxy breakdown, DbContext context) : base(breakdown, context)
        {
        }

        protected override void AddOrUpdateInternal(Composer entity)
        {
            throw new NotImplementedException();
        }

        public override Composer Find(Guid key)
        {
            throw new NotImplementedException();
        }
    }

    internal class BreakdownProxy : DomainTypeMapperBase<Composer, ComposerRelationalDto>
    {
        public BreakdownProxy() :
            base(new Mock<IDtoFactory>().Object)
        {
        }

        protected override IEnumerable<RelationdalDtoBase> BreakdownInternal(Composer entity)
        {
            return new[] { BuildDtoInternal(entity) };
        }

        protected override ComposerRelationalDto BuildDtoInternal(Composer entity)
        {
            return new ComposerRelationalDto();
        }

        protected override Composer BuildInternal(ComposerRelationalDto dto)
        {
            return new Composer();
        }
    }

    public class CtorTests : TestFixtureBase
    {

        [Test]
        public void ThrowsExceptionIfNullBreakdown()
        {
            Assert.Throws<ArgumentNullException>(() => new EFRepoProxy(null, new Mock<DbContext>().Object));
        }

        [Test]
        public void ThrowsExceptionIfNullContext()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new EFRepoProxy(new BreakdownProxy(), null);
            });
        }
    }

    public class AddOrUpdateTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullEntity()
        {
            var repo = new EFRepoProxy(new BreakdownProxy(), new Mock<DbContext>().Object);
            Assert.Throws<ArgumentNullException>(() => repo.AddOrUpdate(null));
        }
    }

    public class DeleteTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullEntity()
        {
            var repo = new EFRepoProxy(new BreakdownProxy(), new Mock<DbContext>().Object);

            Assert.Throws<ArgumentNullException>(() => repo.Delete(null));
        }

        [Test]
        public void DeletesEntities()
        {
            Guid id1 = new Guid(7, 7, 7, new byte[8]);
            Guid id2 = new Guid(8, 8, 8, new byte[8]);
            var db = new List<ComposerRelationalDto>() { new ComposerRelationalDto() { Id = id1 }, new ComposerRelationalDto() { Id = id2 } };
            Mock<DbSet<ComposerRelationalDto>> mockDbSet = GetMockDbSet(db);
            var ctx = new Mock<DbContext>();
            ctx.Setup(d => d.Set<ComposerRelationalDto>()).Returns(() => mockDbSet.Object);
            mockDbSet.Setup(s => s.Find(It.Is((object[] keys) => keys.Length == 1))).Returns((object[] keys) => db.Where(dto => dto.Id == (Guid)keys[0]).FirstOrDefault());
            var repo = new EFRepoProxy(new BreakdownProxy(), ctx.Object);

            repo.Delete(new Composer() { Id = id1 });

            Assert.IsFalse(db.Any(dto => dto.Id == id1));
        }
    }

    public class SaveChangesTests : TestFixtureBase
    {
        [Test]
        public void CallsUnderlyingDbContext()
        {
            Mock<DbContext> ctx = GetMockDbContext();
            //ctx.Setup(c => c.SaveChanges());
            var repo = new EFRepoProxy(new BreakdownProxy(), ctx.Object);

            repo.SaveChanges();

            ctx.Verify(c => c.SaveChanges());
        }
    }
}