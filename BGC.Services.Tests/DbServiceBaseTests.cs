using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BGC.Core.Services;
using Moq;
using System.Runtime.CompilerServices;
using BGC.Services;

namespace BGC.Core.Tests
{
    /// <summary>
    /// Summary description for ServiceBaseTests
    /// </summary>
    [TestClass]
    public class DbServiceBaseTests
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

            public BasicServiceImplementation()
            {
            }
        }

        internal class CustomNonEmptyService : BasicServiceImplementation
        {
            internal IRepository<MockEntity> MockRepo1 { get; set; }
            internal IRepository<MockEntity> MockRepo2 { get; set; }

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
        }

        [TestClass]
        public class CommonUnitOfWorkTests
        {

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
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
                IUnitOfWork commonUnitOfWork = testObj.GetCommonUnitOfWork();
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsExceptionIfNoUnitOfWork()
            {
                BasicServiceImplementation testObj = new BasicServiceImplementation();
                IUnitOfWork commonUnitOfWork = testObj.GetCommonUnitOfWork();
            }

            [TestMethod]
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

            [TestMethod]
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

        [TestClass]
        public class GetDbConnectMembersTest
        {
            [TestMethod]
            public void ReturnsEmptyCollectionWhenNoDbConnectedObjects()
            {
                BasicServiceImplementation svc = new BasicServiceImplementation();
                Assert.IsFalse(svc.GetDbConnectMembers().Any());
            }

            [TestMethod]
            public void ReturnsCorrectDbConnectedMembers()
            {
                IUnitOfWork commonUnitOfWork = new Mock<IUnitOfWork>().Object;
                Mock<IRepository<MockEntity>> mockRepo1 = new Mock<IRepository<MockEntity>>();
                Mock<IRepository<MockEntity>> mockRepo2 = new Mock<IRepository<MockEntity>>();
                mockRepo1.SetupGet(x => x.UnitOfWork).Returns(commonUnitOfWork);
                mockRepo2.SetupGet(x => x.UnitOfWork).Returns(commonUnitOfWork);
                CustomNonEmptyService testObj = new CustomNonEmptyService()
                {
                    MockRepo1 = mockRepo1.Object,
                    MockRepo2 = mockRepo2.Object
                };
                Assert.AreEqual(2, testObj.GetDbConnectMembers().Count());
            }
        }
    }
}