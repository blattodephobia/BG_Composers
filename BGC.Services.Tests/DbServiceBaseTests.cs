using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using BGC.Core.Services;
using Moq;
using System.Runtime.CompilerServices;
using BGC.Services;
using System.Data.Entity;
using BGC.Data;
using TestUtils;

namespace BGC.Core.Tests
{
    public class MockEntity
    {

    }

    internal class BasicServiceImplementation : DbServiceBase
    {
        public IUnitOfWork GetCommonUnitOfWork()
        {
            return this.CommonUnitOfWork;
        }

        public IEnumerable<IDbConnect> GetDbConnectMembers() => GetDatbaseConnectedObjects();

        public IEnumerable<IDbPersist> GetDbConnectMembers2() => GetDatabaseConnectedObjects2();

        public BasicServiceImplementation()
        {
        }
    }

    internal class CustomNonEmptyService : BasicServiceImplementation
    {
        internal IRepository<MockEntity> MockRepo1 { get; set; }
        internal IRepository<MockEntity> MockRepo2 { get; set; }
        internal IDbPersist MockRepo3 { get; set; }

        public CustomNonEmptyService()
        {
        }
    }

    internal class MockUnitOfWork : IUnitOfWork
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void SetState<T>(T entity, EntityState state) where T : class
        {
            throw new NotImplementedException();
        }
    }

    internal class MockUnitOfWorkThrowsOnGetHashCode : IUnitOfWork
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotSupportedException("UnitOfWork equality should not depend on GetHashCode but rather than on the static objec.ReferenceEquals method");
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void SetState<T>(T entity, EntityState state) where T : class
        {
            throw new NotImplementedException();
        }
    }

    [TestFixture]
    public class CommonUnitOfWorkTests
    {
        [Test]
        public void ThrowsExceptionIfMoreThanOneUnitOfWork()
        {
            Mock<IUnitOfWork> unitOfWork1 = new Mock<IUnitOfWork>();
            Mock<IUnitOfWork> unitOfWork2 = new Mock<IUnitOfWork>();
            Mock<IRepository<MockEntity>> mockRepo1 = new Mock<IRepository<MockEntity>>();
            Mock<IRepository<MockEntity>> mockRepo2 = new Mock<IRepository<MockEntity>>();
            mockRepo1.SetupGet(x => x.UnitOfWork).Returns(unitOfWork1.Object);
            mockRepo2.SetupGet(x => x.UnitOfWork).Returns(unitOfWork2.Object);
            CustomNonEmptyService testObj = new CustomNonEmptyService()
            {
                MockRepo1 = mockRepo1.Object,
                MockRepo2 = mockRepo2.Object
            };
            Assert.Throws<InvalidOperationException>(() => testObj.GetCommonUnitOfWork());
        }

        [Test]
        public void ThrowsExceptionIfNoUnitOfWork()
        {
            BasicServiceImplementation testObj = new BasicServiceImplementation();
            Assert.Throws<InvalidOperationException>(() => testObj.GetCommonUnitOfWork());
        }

        [Test]
        public void ReturnsCorrectUnitOfWorkReference()
        {
            IUnitOfWork mockUnitOfWork = new MockUnitOfWork();
            Mock<IRepository<MockEntity>> mockRepo1 = new Mock<IRepository<MockEntity>>();
            Mock<IRepository<MockEntity>> mockRepo2 = new Mock<IRepository<MockEntity>>();
            mockRepo1.SetupGet(x => x.UnitOfWork).Returns(mockUnitOfWork);
            mockRepo2.SetupGet(x => x.UnitOfWork).Returns(mockUnitOfWork);
            CustomNonEmptyService testObj = new CustomNonEmptyService()
            {
                MockRepo1 = mockRepo1.Object,
                MockRepo2 = mockRepo2.Object
            };
            IUnitOfWork commonUnitOfWork = testObj.GetCommonUnitOfWork();
            Assert.AreSame(mockUnitOfWork, commonUnitOfWork);
        }

        [Test]
        public void ReturnsCorrectUnitOfWorkReferenceWithoutGetHashCode()
        {
            IUnitOfWork commonUnitOfWork = new MockUnitOfWorkThrowsOnGetHashCode();
            Mock<IRepository<MockEntity>> mockRepo1 = new Mock<IRepository<MockEntity>>();
            Mock<IRepository<MockEntity>> mockRepo2 = new Mock<IRepository<MockEntity>>();
            mockRepo1.SetupGet(x => x.UnitOfWork).Returns(commonUnitOfWork);
            mockRepo2.SetupGet(x => x.UnitOfWork).Returns(commonUnitOfWork);
            CustomNonEmptyService testObj = new CustomNonEmptyService()
            {
                MockRepo1 = mockRepo1.Object,
                MockRepo2 = mockRepo2.Object
            };
            Assert.AreSame(commonUnitOfWork, testObj.GetCommonUnitOfWork());
        }
    }
    
    public class GetDbConnectMembersTest : TestFixtureBase
    {
        private CustomNonEmptyService _testObj;

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();

            IUnitOfWork commonUnitOfWork = new Mock<IUnitOfWork>().Object;
            Mock<IRepository<MockEntity>> mockRepo1 = new Mock<IRepository<MockEntity>>();
            Mock<IRepository<MockEntity>> mockRepo2 = new Mock<IRepository<MockEntity>>();
            Mock<IDbPersist> mockRepo3 = new Mock<IDbPersist>();
            mockRepo1.SetupGet(x => x.UnitOfWork).Returns(commonUnitOfWork);
            mockRepo2.SetupGet(x => x.UnitOfWork).Returns(commonUnitOfWork);
            _testObj = new CustomNonEmptyService()
            {
                MockRepo1 = mockRepo1.Object,
                MockRepo2 = mockRepo2.Object,
                MockRepo3 = mockRepo3.Object
            };
        }

        [Test]
        public void ReturnsEmptyCollectionWhenNoDbConnectedObjects()
        {
            BasicServiceImplementation svc = new BasicServiceImplementation();
            Assert.IsFalse(svc.GetDbConnectMembers().Any());
        }

        [Test]
        public void ReturnsCorrectDbConnectedMembers()
        {
            Assert.AreEqual(2, _testObj.GetDbConnectMembers().Count());
        }

        [Test]
        public void ReturnsCorrectDbPersistMembers()
        {
            Assert.AreSame(_testObj.MockRepo3, _testObj.GetDbConnectMembers2().Single());
        }
    }
}