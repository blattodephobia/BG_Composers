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

        [TestClass]
        public class GetQueryStringTests
        {
            private static void MockActionMethod1(int param1, string param2)
            {
            }

            private static int MockActionMethod2(int param1, string param2)
            {
                return 0;
            }

            private int field1 = 5;

            [TestMethod]
            public void GeneratesCorrectStringWithConstants()
            {
                string result = Expressions.GetQueryString(() => MockActionMethod1(2, "cimf"));
                Assert.AreEqual("param1=2&param2=cimf", result);
            }

            [TestMethod]
            public void GeneratesCorrectStringWithVariables()
            {
                int value1 = 2;
                string value2 = "cimf";
                string result = Expressions.GetQueryString(() => MockActionMethod1(value1, value2));
                Assert.AreEqual("param1=2&param2=cimf", result);
            }

            [TestMethod]
            public void GeneratesCorrectStringWithMixedConstantsAndVariables1()
            {
                string value2 = "cimf";
                string result = Expressions.GetQueryString(() => MockActionMethod1(2, value2));
                Assert.AreEqual("param1=2&param2=cimf", result);
            }

            [TestMethod]
            public void GeneratesCorrectStringWithMixedConstantsAndVariables2()
            {
                string value2 = "cimf";
                string result = Expressions.GetQueryString(() => MockActionMethod2(2, value2));
                Assert.AreEqual("param1=2&param2=cimf", result);
            }

            [TestMethod]
            public void GeneratesCorrectStringWithMixedConstantsAndVariables_ThisCall()
            {
                string value2 = "cimf";
                string result = Expressions.GetQueryString(() => MockActionMethod2(this.field1, value2));
                Assert.AreEqual("param1=5&param2=cimf", result);
            }

            [TestMethod]
            public void GeneratesCorrectStringWithMixedConstantsAndVariables_IncludeNullValue()
            {
                string result = Expressions.GetQueryString(() => MockActionMethod2(this.field1, null), includeNullValueParams: true);
                Assert.AreEqual("param1=5&param2=", result);
            }

            [TestMethod]
            public void GeneratesCorrectStringWithMixedConstantsAndVariables_ExcludeNullValue()
            {
                string result = Expressions.GetQueryString(() => MockActionMethod2(this.field1, null), includeNullValueParams: false);
                Assert.AreEqual("param1=5", result);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsOnNestedMethodCallExpressions()
            {
                string value2 = "cimf";
                string result = Expressions.GetQueryString(() => MockActionMethod1(MockActionMethod2(2, ""), value2));
            }
        }
	}
}
