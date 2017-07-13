using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities.Tests
{
    [TestFixture]
    public class LazyOrderedEnumerableTests
    {
        private class Entity
        {
            public int Key { get; private set; }

            public object Data { get; set; }

            public Entity(int key)
            {
                Key = key;
            }
        }

        [Test]
        public void SortsSingleElementCollection()
        {
            IEnumerable<int> result = new LazyOrderedEnumerable<int, int>(new int[] { 1 }, x => x, Comparer<int>.Default, false);
            Assert.AreEqual(1, result.First());
        }

        [Test]
        public void SortsDoubleElementCollection()
        {
            IEnumerable<int> result = new LazyOrderedEnumerable<int, int>(new int[] { 5, 1 }, x => x, Comparer<int>.Default, false);
            Assert.AreEqual(1, result.First());
        }

        [Test]
        public void SortsTripleElementCollection()
        {
            IEnumerable<int> result = new LazyOrderedEnumerable<int, int>(new int[] { 5, 6, 1 }, x => x, Comparer<int>.Default, false);
            Assert.AreEqual(1, result.First());
        }

        [Test]
        public void SortsArbitraryCollection()
        {
            IEnumerable<int> result = new LazyOrderedEnumerable<int, int>(new int[] { 5, 6, 1, -1, 30, 0, 3, 12 }, x => x, Comparer<int>.Default, false);
            int previousElement = result.First();
            foreach (int elem in result.Skip(1))
            {
                if (elem < previousElement)
                {
                    Assert.Fail("Sequence is not sorted.");
                }
            }
        }

        [Test]
        public void SortsWithKeySelector()
        {
            IEnumerable<Entity> result = new LazyOrderedEnumerable<Entity, int>(new []
            {
                new Entity(5),
                new Entity(6),
                new Entity(1),
                new Entity(-1),
                new Entity(30),
                new Entity(0),
                new Entity(3),
                new Entity(12)
            },
            x => x.Key);

            int previousElement = result.First().Key;
            foreach (Entity elem in result.Skip(1))
            {
                if (elem.Key < previousElement)
                {
                    Assert.Fail("Sequence is not sorted properly.");
                }
            }
        }

        [Test]
        public void ThrowsExceptionIfNoApplicableComparer()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                IEnumerable<Entity> result = new LazyOrderedEnumerable<Entity, Entity>(new[]
                {
                new Entity(5),
                new Entity(6),
                new Entity(1),
                new Entity(-1),
                new Entity(30),
                new Entity(0),
                new Entity(3),
                new Entity(12)
            },
                x => x);
            });
        }

        [Test]
        public void ThrowsExceptionIfNullKeySelector()
        {
            Assert.Throws<ArgumentNullException>(() => new LazyOrderedEnumerable<int, int>(new int[0], null, Comparer<int>.Default, false));
        }

        [Test]
        public void SortsArbitraryCollectionDescending()
        {
            IEnumerable<int> result = new LazyOrderedEnumerable<int, int>(new int[] { 5, 6, 1, -1, 30, 0, 3, 12 }, x => x, Comparer<int>.Default, descending: true);
            int previousElement = result.First();
            foreach (int elem in result.Skip(1))
            {
                if (elem > previousElement)
                {
                    Assert.Fail("Sequence is not sorted properly.");
                }
            }
        }
    }
}
