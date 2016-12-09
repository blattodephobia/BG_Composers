﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BGC.Utilities.Tests
{
    [TestFixture]
    public class NameOfTests
    {
        static string StaticProperty { get; set; }

        const int Constant = 0;

        string TestProperty { get; set; }

        string testField = null;

        int TestValueTypeProperty { get; set; }

        [Test]
        public void ReturnsInstanceMemberName()
        {
            string actual = Expressions.NameOf(() => this.TestProperty);
            Assert.AreEqual("TestProperty", actual);
        }

        [Test]
        public void ReturnsInstanceMemberNameWithBoxing()
        {
            string actual = Expressions.NameOf<object>(() => this.TestValueTypeProperty);
            Assert.AreEqual("TestValueTypeProperty", actual);
        }

        [Test]
        public void ReturnsInstanceFieldName()
        {
            string actual = Expressions.NameOf(() => this.testField);
            Assert.AreEqual("testField", actual);
        }

        [Test]
        public void ReturnsNonInstantiatedMemberName()
        {
            string actual = Expressions.NameOf<NameOfTests>((obj) => obj.TestProperty);
            Assert.AreEqual("TestProperty", actual);
        }

        [Test]
        public void ReturnsNonInstantiatedMemberNameWithBoxing()
        {
            string actual = Expressions.NameOf<NameOfTests>((obj) => obj.TestValueTypeProperty);
            Assert.AreEqual("TestValueTypeProperty", actual);
        }

        [Test]
        public void ReturnsNonInstantiatedFieldName()
        {
            string actual = Expressions.NameOf<NameOfTests>((obj) => obj.testField);
            Assert.AreEqual("testField", actual);
        }

        [Test]
        public void ReturnsStaticPropertyName()
        {
            string actual = Expressions.NameOf(() => StaticProperty);
            Assert.AreEqual("StaticProperty", actual);
            actual = Expressions.NameOf(() => NameOfTests.StaticProperty);
            Assert.AreEqual("StaticProperty", actual);
        }

        [Test]
        public void ThrowsOnConstantExpression()
        {
            Assert.Throws<InvalidOperationException>(() => Expressions.NameOf(() => NameOfTests.Constant));
        }
    }

    [TestFixture]
    public class GetQueryStringTests
    {
        private class CaptureClass
        {
            public string Property { get; set; }
        }

        private class CaptureClass2
        {
            public CaptureClass NestedProperty { get; set; }
        }

        private static void MockActionMethod1(int param1, string param2)
        {
        }

        private int MockActionMethod2(int param1, string param2)
        {
            return 0;
        }

        private int field1 = 5;

        [Test]
        public void GeneratesCorrectStringWithConstants()
        {
            string result = Expressions.GetQueryString(() => MockActionMethod1(2, "cimf"));
            Assert.AreEqual("param1=2&param2=cimf", result);
        }

        [Test]
        public void GeneratesCorrectStringWithVariables()
        {
            int value1 = 2;
            string value2 = "cimf";
            string result = Expressions.GetQueryString(() => MockActionMethod1(value1, value2));
            Assert.AreEqual("param1=2&param2=cimf", result);
        }

        [Test]
        public void GeneratesCorrectStringWithMixedConstantsAndVariables1()
        {
            string value2 = "cimf";
            string result = Expressions.GetQueryString(() => MockActionMethod1(2, value2));
            Assert.AreEqual("param1=2&param2=cimf", result);
        }

        [Test]
        public void GeneratesCorrectStringWithMixedConstantsAndVariables2()
        {
            string value2 = "cimf";
            string result = Expressions.GetQueryString(() => MockActionMethod2(2, value2));
            Assert.AreEqual("param1=2&param2=cimf", result);
        }

        [Test]
        public void GeneratesCorrectStringWithMixedConstantsAndVariables_ThisCall()
        {
            string value2 = "cimf";
            string result = Expressions.GetQueryString(() => MockActionMethod2(this.field1, value2));
            Assert.AreEqual("param1=5&param2=cimf", result);
        }

        [Test]
        public void GeneratesCorrectStringWithMixedConstantsAndVariables_IncludeNullValue()
        {
            string result = Expressions.GetQueryString(() => MockActionMethod2(this.field1, null), includeNullValueParams: true);
            Assert.AreEqual("param1=5&param2=", result);
        }

        [Test]
        public void GeneratesCorrectStringWithNestedMemberAccess1()
        {
            CaptureClass cc = new CaptureClass() { Property = "1" };
            Assert.AreEqual("param1=2&param2=1", Expressions.GetQueryString(() => MockActionMethod2(2, cc.Property)));
        }

        [Test]
        public void GeneratesCorrectStringWithNestedMemberAccess2()
        {
            CaptureClass2 cc2 = new CaptureClass2() { NestedProperty = new CaptureClass() { Property = "1" } };
            Assert.AreEqual("param1=2&param2=1", Expressions.GetQueryString(() => MockActionMethod2(2, cc2.NestedProperty.Property)));
        }

        [Test]
        public void GeneratesCorrectStringWithMixedConstantsAndVariables_ExcludeNullValue()
        {
            string result = Expressions.GetQueryString((GetQueryStringTests t) => t.MockActionMethod2(this.field1, null), includeNullValueParams: false);
            Assert.AreEqual("param1=5", result);
        }

        [Test]
        public void ThrowsOnNestedMethodCallExpressions()
        {
            string value2 = "cimf";
            Assert.Throws<InvalidOperationException>(() => Expressions.GetQueryString(() => MockActionMethod1(MockActionMethod2(2, ""), value2)));
        }
    }

    [TestFixture]
    public class GetPropertyValuesOfTypeAccessorTests
    {
        internal class CustomNonEmptyService
        {
            internal List<int> MockRepo1 { get; set; }
            internal List<int> MockRepo2 { get; set; }
        }

        internal struct CustomNonEmptyStructService
        {
            internal double MockProperty1 { get; set; }
            internal double MockProperty2 { get; set; }
        }

        internal class CustomEmptyService
        {
            public CustomEmptyService()
            {
            }
        }

        [Test]
        public void ShouldReturnAllMatchingProperties()
        {
            Func<object, IEnumerable<IEnumerable<int>>> accessor = Expressions.GetPropertyValuesOfTypeAccessor<IEnumerable<int>>(typeof(CustomNonEmptyService));
            Assert.AreEqual(2, accessor.Invoke(new CustomNonEmptyService()).Count());
        }

        [Test]
        public void ShouldReturnAllMatchingPropertiesForValueTypes()
        {
            Func<object, IEnumerable<double>> accessor = Expressions.GetPropertyValuesOfTypeAccessor<double>(typeof(CustomNonEmptyStructService));
            Assert.AreEqual(2, accessor.Invoke(new CustomNonEmptyStructService()).Count());
        }

        [Test]
        public void ShouldReturnEmptyEnumerable()
        {
            Func<object, IEnumerable<object>> accessor = Expressions.GetPropertyValuesOfTypeAccessor<object>(typeof(CustomEmptyService));
            Assert.AreEqual(0, accessor.Invoke(new CustomEmptyService()).Count());
        }
    }
}
