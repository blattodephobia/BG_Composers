using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TestUtils;

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

            public static string StaticProperty => "StaticFieldProperty";

            public static readonly string StaticField = "StaticFieldValue";
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
        public void GeneratesCorrectStringWithStaticProperty()
        {
            string actual = Expressions.GetQueryString(() => MockActionMethod2(2, CaptureClass2.StaticProperty));
            Assert.AreEqual($"param1=2&param2={CaptureClass2.StaticProperty}", actual);
        }

        [Test]
        public void GeneratesCorrectStringWithStaticField()
        {
            string actual = Expressions.GetQueryString(() => MockActionMethod2(2, CaptureClass2.StaticField));
            Assert.AreEqual($"param1=2&param2={CaptureClass2.StaticField}", actual);
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

        internal class StaticPropertiesClass
        {
            public static int StaticProperty1 { get; private set; } = 2;
            public static int StaticProperty2 { get; private set; } = 5;
        }

        internal class MixedPropertiesClass
        {
            public static int StaticProperty { get; private set; } = 2;
            public int InstanceProperty { get; private set; } = 5;

            public static Expression<Func<MixedPropertiesClass, IEnumerable<int>>> Test = (MixedPropertiesClass x) =>
                x == null
            ? new[] { MixedPropertiesClass.StaticProperty }
            : new[] { x.InstanceProperty };
        }

        [Test]
        public void ShouldReturnAllMatchingProperties()
        {
            Func<CustomNonEmptyService, IEnumerable<IEnumerable<int>>> accessor = Expressions.GetPropertyValuesOfTypeAccessor<CustomNonEmptyService, IEnumerable<int>>();
            Assert.AreEqual(2, accessor.Invoke(new CustomNonEmptyService()).Count());
        }

        [Test]
        public void ShouldReturnAllMatchingPropertiesForValueTypes()
        {
            Func<CustomNonEmptyStructService, IEnumerable<double>> accessor = Expressions.GetPropertyValuesOfTypeAccessor<CustomNonEmptyStructService, double>();
            Assert.AreEqual(2, accessor.Invoke(new CustomNonEmptyStructService()).Count());
        }

        [Test]
        public void ShouldReturnEmptyEnumerable()
        {
            Func<CustomEmptyService, IEnumerable<object>> accessor = Expressions.GetPropertyValuesOfTypeAccessor<CustomEmptyService, object>();
            Assert.AreEqual(0, accessor.Invoke(new CustomEmptyService()).Count());
        }

        [Test]
        public void ReturnsStaticPropertiesIf()
        {
            Func<StaticPropertiesClass, IEnumerable<int>> accessor = Expressions.GetPropertyValuesOfTypeAccessor<int>(typeof(StaticPropertiesClass));
            IEnumerable<int> expected = new int[] { StaticPropertiesClass.StaticProperty1, StaticPropertiesClass.StaticProperty2 }.OrderBy(x => x);
            IEnumerable<int> actual = accessor.Invoke(null).OrderBy(x => x);
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [Test]
        public void ReturnsStaticProperties_NullInstanceMixedPropertyModifiers()
        {
            Func<MixedPropertiesClass, IEnumerable<int>> accessor = Expressions.GetPropertyValuesOfTypeAccessor<int>(typeof(MixedPropertiesClass));

            IEnumerable<int> expected = new int[] { MixedPropertiesClass.StaticProperty };
            IEnumerable<int> actual = accessor.Invoke(null);

            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(expected.Single(), actual.Single());
        }
    }

    public class GetPropertiesTests : TestFixtureBase
    {
        internal class Base
        {
            public string ReferenceType { get; set; }

            public int ValueType { get; set; }
        }

        internal class Derived : Base
        {
            public char ChildProperty { get; set; }
        }

        [Test]
        public void ThrowsExceptionIfNullCollections()
        {
            Assert.Throws<ArgumentNullException>(() => Expressions.GetPropertiesAccessor<GetPropertiesTests>(null));
        }

        [Test]
        public void ThrowsExceptionIfPropertiesFromDifferentType()
        {
            Assert.Throws<AmbiguousMatchException>(() => Expressions.GetPropertiesAccessor<GetPropertiesTests>(typeof(List<>).GetProperties()));
        }

        [Test]
        public void GetsPropertiesOfBasicType()
        {
            Base obj = new Base() { ReferenceType = "hi", ValueType = 7 };
            object[] propertyValues = Expressions.GetPropertiesAccessor<Base>(typeof(Base).GetProperties()).Invoke(obj);

            Assert.IsTrue(propertyValues.FirstOrDefault(o => o.Equals(obj.ValueType)) != null);
            Assert.IsTrue(propertyValues.FirstOrDefault(o => o.Equals(obj.ReferenceType)) != null);
        }

        [Test]
        public void GetsPropertiesOfDerivedType()
        {
            Derived obj = new Derived() { ReferenceType = "hi", ValueType = 7, ChildProperty = 'N' };
            object[] propertyValues = Expressions.GetPropertiesAccessor<Derived>(typeof(Derived).GetProperties()).Invoke(obj);

            Assert.IsTrue(propertyValues.FirstOrDefault(o => o.Equals(obj.ValueType)) != null);
            Assert.IsTrue(propertyValues.FirstOrDefault(o => o.Equals(obj.ReferenceType)) != null);
            Assert.IsTrue(propertyValues.FirstOrDefault(o => o.Equals(obj.ChildProperty)) != null);
        }
    }
}
