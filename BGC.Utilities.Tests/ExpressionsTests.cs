using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BGC.Utilities.Tests
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
    public class GetPropertyValuesOfTypeAccessorTests
    {
        internal class EmptyClass
        {
        }

        internal class NonEmptyClass
        {
            internal int MockProperty1 { get; set; }
            internal int MockProperty2 { get; set; }
            internal double MockProperty3 { get; set; }
        }

        [TestMethod]
        public void ReturnsCollectionWithCorrectValues1()
        {
            var func = Expressions.GetPropertyValuesOfTypeAccessor<NonEmptyClass, int>();
            var result = func.Invoke(new NonEmptyClass() { MockProperty1 = 1, MockProperty2 = 2 }).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(1));
            Assert.IsTrue(result.Contains(2));
        }
        
        [TestMethod]
        public void ReturnsCollectionWithCorrectValues2()
        {
            var func = Expressions.GetPropertyValuesOfTypeAccessor<NonEmptyClass, double>();
            var result = func.Invoke(new NonEmptyClass() { MockProperty1 = 1, MockProperty2 = 2, MockProperty3 = 4.5 }).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains(4.5));
        }
        
        [TestMethod]
        public void ReturnsEmptyCollectionWhenNoMatches()
        {
            var func = Expressions.GetPropertyValuesOfTypeAccessor<NonEmptyClass, char>();
            var result = func.Invoke(new NonEmptyClass() { MockProperty1 = 1, MockProperty2 = 2, MockProperty3 = 4.5 }).ToList();
            Assert.AreEqual(0, result.Count);
        }
    }
}
