using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Utilities.Tests.EnumerableExtensionsTests
{
    [TestFixture]
    public class ToStringAggregateTests
    {
        [Test]
        public void AggregatesWithNoDelimiter()
        {
            string[] enumerable = new[] { "1", "2", "3" };
            string aggregate = enumerable.ToStringAggregate();
            Assert.AreEqual("123", aggregate);
        }

        [Test]
        public void AggregatesWithDelimiter()
        {
            string[] enumerable = new[] { "1", "2", "3" };
            string aggregate = enumerable.ToStringAggregate("#");
            Assert.AreEqual("1#2#3", aggregate);
        }

        [Test]
        public void AggregatesWithSingleElementNoDelimiter()
        {
            string[] enumerable = new[] { "1" };
            string aggregate = enumerable.ToStringAggregate();
            Assert.AreEqual("1", aggregate);
        }

        [Test]
        public void AggregatesWithSingleElementWithDelimiter()
        {
            string[] enumerable = new[] { "1" };
            string aggregate = enumerable.ToStringAggregate("asd");
            Assert.AreEqual("1", aggregate);
        }

        [Test]
        public void ThrowsOnNullCollection()
        {
            IEnumerable<string> nullCollection = null;
            Assert.Throws<ArgumentNullException>(() => nullCollection.ToStringAggregate("asd"));
        }
    }

    public class AddRangeTests : TestFixtureBase
    {
        [Test]
        public void AddsItemsIfCollection()
        {
            var coll = new Collection<int>();
            var items = new[] { 1, 2, 3 };
            EnumerableExtensions.AddRange(coll, items);

            Assert.IsTrue(items.SequenceEqual(coll));
        }

        [Test]
        public void AddsItemsIfList()
        {
            ICollection<int> list = new List<int>();
            var items = new[] { 1, 2, 3 };
            EnumerableExtensions.AddRange(list, items);

            Assert.IsTrue(items.SequenceEqual(list));

        }

        [Test]
        public void ThrowsExceptionIfNullCollection()
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.AddRange(null, new[] { 2, 3 }));
        }

        [Test]
        public void ThrowsExceptionIfNullItems()
        {
            var coll = new Collection<int>();
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.AddRange(coll, null));
        }
    }

    public class IndexOfTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullCollection()
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.IndexOf<object>(null, x => true));
        }

        [Test]
        public void ThrowsExceptionIfNullPredicate()
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.IndexOf(new object[1], null));
        }

        [Test]
        public void ReturnsNegativeOneIfNoMatch_EmptyCollection()
        {
            int index = EnumerableExtensions.IndexOf(new object[0], x => true);

            Assert.AreEqual(-1, index);
        }

        [Test]
        public void ReturnsNegativeOneIfNoMatch_EmptyEnumerable()
        {
            int index = EnumerableExtensions.IndexOf(new int[0].Select(x => x), x => true);

            Assert.AreEqual(-1, index);
        }

        [Test]
        public void ReturnsNegativeOneIfNoMatch_NonEmptyCollection()
        {
            int index = EnumerableExtensions.IndexOf(new int[] { 1, 2, 3 }, x => x == 0);

            Assert.AreEqual(-1, index);
        }

        [Test]
        public void ReturnsNegativeOneIfNoMatch_NonEmptyEnumerable()
        {
            int index = EnumerableExtensions.IndexOf(new int[] { 1, 2, 3 }.Select(x => x), x => x == 0);

            Assert.AreEqual(-1, index);
        }

        [Test]
        public void FindsIndex_List()
        {
            var list = new List<int>() { 2, 3, 4 };

            Assert.AreEqual(1, list.IndexOf(x => x == 3));
        }

        [Test]
        public void FindsIndex_Enumerable()
        {
            IEnumerable<int> list = new List<int>() { 2, 3, 4 }.Select(x => x);

            Assert.AreEqual(1, list.IndexOf(x => x == 3));
        }
    }
}
