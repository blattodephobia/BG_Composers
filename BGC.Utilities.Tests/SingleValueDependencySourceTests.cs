using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities.Tests
{
    [TestFixture]
    public class OnEffectiveValueChangedTests
    {
        [Test]
        public void RaisesEventOnSetValue()
        {
            SingleValueDependencySource<int> src = new SingleValueDependencySource<int>();
            int newValue = 0;
            int invocationsCount = 0;
            src.EffectiveValueChanged += (s, e) =>
            {
                invocationsCount++;
                newValue = e.NewValue;
            };
            src.SetValue(25);

            Assert.AreEqual(25, newValue);
            Assert.AreEqual(1, invocationsCount);
        }

        [Test]
        public void RaisesEventOnUnsetValue()
        {
            SingleValueDependencySource<int> src = new SingleValueDependencySource<int>();
            int newValue = 0;
            int invocationsCount = 0;
            src.SetValue(25);
            src.EffectiveValueChanged += (s, e) =>
            {
                invocationsCount++;
                newValue = e.NewValue;
            };
            src.UnsetValue();

            Assert.IsFalse(src.HasValue);
            Assert.AreEqual(0, src.GetEffectiveValue());
            Assert.AreEqual(1, invocationsCount);
        }
    }

    [TestFixture]
    public class SetUnsetValueTests
    {
        [Test]
        public void SetsValue()
        {
            SingleValueDependencySource<int> src = new SingleValueDependencySource<int>();
            src.SetValue(34);
            Assert.AreEqual(34, src.GetEffectiveValue());
        }

        [Test]
        public void UnSetsValue()
        {
            SingleValueDependencySource<int> src = new SingleValueDependencySource<int>();
            src.SetValue(34);
            src.UnsetValue();
            Assert.AreEqual(0, src.GetEffectiveValue());
            Assert.IsFalse(src.HasValue);
        }
    }

    [TestFixture]
    public class DefaultValueAllowedTests
    {
        [Test]
        public void HasValueIfDefaultValueIsAllowedAndSet_ValueType()
        {
            SingleValueDependencySource<int> src = new SingleValueDependencySource<int>(true);
            Assert.IsFalse(src.HasValue);
            src.SetValue((default(int)));
            Assert.IsTrue(src.HasValue);
        }

        [Test]
        public void HasNoValueIfDefaultIsNotAllowedAndSet_ValueType()
        {
            SingleValueDependencySource<int> src = new SingleValueDependencySource<int>(false);
            Assert.IsFalse(src.HasValue);
            src.SetValue((default(int)));
            Assert.IsFalse(src.HasValue);
        }

        [Test]
        public void HasValueIfDefaultValueIsAllowedAndSet_ReferenceType()
        {
            SingleValueDependencySource<string> src = new SingleValueDependencySource<string>(true);
            Assert.IsFalse(src.HasValue);
            src.SetValue((default(string)));
            Assert.IsTrue(src.HasValue);
        }

        [Test]
        public void HasNoValueIfDefaultIsNotAllowedAndSet_ReferenceType()
        {
            SingleValueDependencySource<string> src = new SingleValueDependencySource<string>(false);
            Assert.IsFalse(src.HasValue);
            src.SetValue((default(string)));
            Assert.IsFalse(src.HasValue);
        }
    }
}
