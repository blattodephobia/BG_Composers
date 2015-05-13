using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BGC.Core.Services;
using Moq;
using System.Runtime.CompilerServices;

namespace BGC.Core.Tests
{
	/// <summary>
	/// Summary description for ServiceBaseTests
	/// </summary>
	[TestClass]
	public class ServiceBaseTests
	{
		[TestClass]
		public class GetPropertyValuesOfTypeAccessorTests
		{
			public class CustomNonEmptyService
			{
				internal IRepository<AspNetUser> MockRepo1 { get; set; }
				internal IRepository<AspNetUser> MockRepo2 { get; set; }

				public CustomNonEmptyService()
				{
					Mock<IUnitOfWork> commonUnitOfWork = new Mock<IUnitOfWork>();
					Mock<IRepository<AspNetUser>> mockRepo1 = new Mock<IRepository<AspNetUser>>();
					Mock<IRepository<AspNetUser>> mockRepo2 = new Mock<IRepository<AspNetUser>>();
					mockRepo1.SetupGet(x => x.UnitOfWork).Returns(commonUnitOfWork.Object);
					mockRepo2.SetupGet(x => x.UnitOfWork).Returns(commonUnitOfWork.Object);
					this.MockRepo1 = mockRepo1.Object;
					this.MockRepo2 = mockRepo2.Object;
				}
			}
			public struct CustomNonEmptyStructService
			{
				internal IRepository<AspNetUser> MockRepo1 { get; set; }
				internal IRepository<AspNetUser> MockRepo2 { get; set; }

				public CustomNonEmptyStructService(bool dummy) :
					this()
				{
					Mock<IUnitOfWork> commonUnitOfWork = new Mock<IUnitOfWork>();
					Mock<IRepository<AspNetUser>> mockRepo1 = new Mock<IRepository<AspNetUser>>();
					Mock<IRepository<AspNetUser>> mockRepo2 = new Mock<IRepository<AspNetUser>>();
					mockRepo1.SetupGet(x => x.UnitOfWork).Returns(commonUnitOfWork.Object);
					mockRepo2.SetupGet(x => x.UnitOfWork).Returns(commonUnitOfWork.Object);
					this.MockRepo1 = mockRepo1.Object;
					this.MockRepo2 = mockRepo2.Object;
				}
			}

			public class CustomEmptyService
			{
				public CustomEmptyService()
				{
				}
			}

			public PrivateType DbConnectedObjectsAccessorHelper { get; set; }

			[TestInitialize]
			public void TestInit()
			{
				this.DbConnectedObjectsAccessorHelper = new PrivateType(typeof(ServiceBase));
			}

			[TestMethod]
			public void ShouldReturnAllMatchingProperties()
			{
				Func<object, IEnumerable<IDbConnect>> accessor = this.DbConnectedObjectsAccessorHelper.InvokeStatic("GetPropertyValuesOfTypeAccessor", new Type[] { typeof(Type) }, new object[] { typeof(CustomNonEmptyService) } , new Type[] { typeof(IDbConnect) }) as Func<object, IEnumerable<IDbConnect>>;
				Assert.AreEqual(2, accessor.Invoke(new CustomNonEmptyService()).Count());
			}

			[TestMethod]
			public void ShouldReturnAllMatchingPropertiesForValueTypes()
			{
				Func<object, IEnumerable<IDbConnect>> accessor = this.DbConnectedObjectsAccessorHelper.InvokeStatic("GetPropertyValuesOfTypeAccessor", new Type[] { typeof(Type) }, new object[] { typeof(CustomNonEmptyStructService) } , new Type[] { typeof(IDbConnect) }) as Func<object, IEnumerable<IDbConnect>>;
				Assert.AreEqual(2, accessor.Invoke(new CustomNonEmptyStructService()).Count());
			}

			[TestMethod]
			public void ShouldReturnEmptyEnumerable()
			{
				Func<object, IEnumerable<IDbConnect>> accessor = this.DbConnectedObjectsAccessorHelper.InvokeStatic("GetPropertyValuesOfTypeAccessor", new Type[] { typeof(Type) }, new object[] { typeof(CustomEmptyService) }, new Type[] { typeof(IDbConnect) }) as Func<object, IEnumerable<IDbConnect>>;
				Assert.AreEqual(0, accessor.Invoke(new CustomEmptyService()).Count());
			}
		}
	}
}