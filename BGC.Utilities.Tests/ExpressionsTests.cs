using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace BGC.Utilities.Tests
{
	[TestClass]
	public class ExpressionsTests
	{
		[TestClass]
		public class NameOfTests
		{
			static string StaticProperty { get; set; }

			const int Constant = 0;

			string TestProperty { get; set; }
            
			string testField = null;

			int TestValueTypeProperty { get; set; }

			[TestMethod]
			public void ReturnsInstanceMemberName()
			{
				string actual = Expressions.NameOf(() => this.TestProperty);
				Assert.AreEqual("TestProperty", actual);
			}

			[TestMethod]
			public void ReturnsInstanceMemberNameWithBoxing()
			{
				string actual = Expressions.NameOf<object>(() => this.TestValueTypeProperty);
				Assert.AreEqual("TestValueTypeProperty", actual);
			}

			[TestMethod]
			public void ReturnsInstanceFieldName()
			{
				string actual = Expressions.NameOf(() => this.testField);
				Assert.AreEqual("testField", actual);
			}

			[TestMethod]
			public void ReturnsNonInstantiatedMemberName()
			{
				string actual = Expressions.NameOf<NameOfTests>((obj) => obj.TestProperty);
				Assert.AreEqual("TestProperty", actual);
			}

			[TestMethod]
			public void ReturnsNonInstantiatedMemberNameWithBoxing()
			{
				string actual = Expressions.NameOf<NameOfTests>((obj) => obj.TestValueTypeProperty);
				Assert.AreEqual("TestValueTypeProperty", actual);
			}

			[TestMethod]
			public void ReturnsNonInstantiatedFieldName()
			{
				string actual = Expressions.NameOf<NameOfTests>((obj) => obj.testField);
				Assert.AreEqual("testField", actual);
			}

			[TestMethod]
			public void ReturnsStaticPropertyName()
			{
				string actual = Expressions.NameOf(() => StaticProperty);
				Assert.AreEqual("StaticProperty", actual);
				actual = Expressions.NameOf(() => NameOfTests.StaticProperty);
				Assert.AreEqual("StaticProperty", actual);
			}

			[TestMethod]
			[ExpectedException(typeof(InvalidOperationException))]
			public void ThrowsOnConstantExpression()
			{
				string actual = Expressions.NameOf(() => Constant);
				Assert.AreEqual("Constant", actual);
				actual = Expressions.NameOf(() => NameOfTests.Constant);
				Assert.AreEqual("Constant", actual);
			}
		}
	}
}
