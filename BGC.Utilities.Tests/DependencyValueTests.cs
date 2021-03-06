﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities.Tests
{
    internal class DependencyValueProxy : DependencyValue<int>
    {
        private readonly Func<int, int> _coerceCallback;

        [DependencyPrecedence(0)]
        public SingleValueDependencySource<int> Source1 { get; set; } = new SingleValueDependencySource<int>();

        [DependencyPrecedence(1)]
        public DependencySource<int> Source2 { get; set; } = new MultiValueFifoDependencySource<int>();

        public DependencySource<int> NoPrecedenceSource { get; private set; }
        
        public DependencySource<bool> WrongSourceType { get; private set; }

        public IEnumerable<DependencySource<int>> GetDependencySourcesProxy() => GetDependencySources();

        protected override int CoerceValue(int value) => _coerceCallback?.Invoke(value) ?? value;

        public DependencyValueProxy(Func<int, int> coerceCallback = null, int defaultValue = 0) :
            base(defaultValue)
        {
            _coerceCallback = coerceCallback;
        }
    }

    [TestFixture]
    public class GetDependencySourcesTests
    {

        [Test]
        public void ReturnsCorrectNumberOfDependencySources()
        {
            DependencyValueProxy proxy = new DependencyValueProxy();
            Assert.AreEqual(3, proxy.GetDependencySourcesProxy().Count());
        }

        [Test]
        public void ReturnsDependencySourcesInCorrectOrder()
        {
            DependencyValueProxy proxy = new DependencyValueProxy();
            List<DependencySource<int>> sources = proxy.GetDependencySourcesProxy().ToList();
            Assert.IsTrue(sources[0] is MultiValueFifoDependencySource<int>);
            Assert.IsTrue(sources[1] is SingleValueDependencySource<int>);
            Assert.IsTrue(sources[2] is DefaultValueDependencySource<int>);
        }
    }

    [TestFixture]
    public class EffectiveValueTests
    {
        private class DependencyValueTestHelper : DependencyValue<int>
        {
            [DependencyPrecedence(1)]
            public SingleValueDependencySource<int> Source1 { get; private set; }

            [DependencyPrecedence(0)]
            public SingleValueDependencySource<int> Source2 { get; private set; }

            public DependencyValueTestHelper() :
                this(0)
            {
            }

            public DependencyValueTestHelper(int defaultValue) :
                base(defaultValue)
            {
                Source1 = new SingleValueDependencySource<int>();
                Source2 = new SingleValueDependencySource<int>();
            }
        }

        [Test]
        public void GetsHighestPriorityValueFirst1()
        {
            DependencyValueTestHelper val = new DependencyValueTestHelper();
            val.Source1.SetValue(23);
            Assert.AreEqual(23, val.EffectiveValue);
        }

        [Test]
        public void GetsHighestPriorityValueFirst2()
        {
            DependencyValueTestHelper val = new DependencyValueTestHelper();
            val.Source2.SetValue(10);
            val.Source1.SetValue(23);
            Assert.AreEqual(23, val.EffectiveValue);
        }

        [Test]
        public void ReturnsDefaultValueWhenNoneIsSet()
        {
            DependencyValueTestHelper val = new DependencyValueTestHelper(17);
            Assert.AreEqual(17, val.EffectiveValue);
        }

        [Test]
        public void ReturnsDefaultValueWhenAllAreUnset()
        {
            DependencyValueTestHelper val = new DependencyValueTestHelper(17);
            val.Source1.SetValue(2);
            Assert.AreEqual(2, val.EffectiveValue);
            val.Source1.UnsetValue();
            Assert.AreEqual(17, val.EffectiveValue);
        }

        [Test]
        public void IgnoresNullSources()
        {
            DependencyValueProxy proxy = new DependencyValueProxy();
            proxy.Source1 = null;
            proxy.Source2.SetValue(12);

            Assert.AreEqual(12, proxy.EffectiveValue);
        }

        [Test]
        public void UpdatesEffectiveValue()
        {
            DependencyValueProxy proxy = new DependencyValueProxy();
            proxy.Source2.SetValue(23);

            Assert.AreEqual(23, proxy.EffectiveValue);
        }
    }

    [TestFixture]
    public class CoercionTests
    {
        [Test]
        public void EffectiveValueIsCoerced()
        {
            var test = new DependencyValueProxy((int i) => Math.Min(i, 100));
            test.Source1.SetValue(34);
            test.Source2.SetValue(200);

            Assert.AreEqual(34, test.EffectiveValue);
        }

        [Test]
        public void EffectiveValueIsCoercedToDefaultValue_DefaultValueIsNotCoercable()
        {
            var test = new DependencyValueProxy((int i) => Math.Min(i, 100), 20);
            test.Source1.SetValue(340);
            test.Source2.SetValue(200);

            Assert.AreEqual(20, test.EffectiveValue);
        }

        [Test]
        public void EffectiveValueIsCoercedToDefaultValue_DefaultValueIsCoercable()
        {
            var test = new DependencyValueProxy((int i) => Math.Min(i, 100), 200);
            test.Source1.SetValue(340);
            test.Source2.SetValue(400);

            Assert.AreEqual(200, test.EffectiveValue);
        }
    }
}
