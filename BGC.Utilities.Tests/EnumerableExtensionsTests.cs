using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities.Tests
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        [TestClass]
        public class ToStringAggregateTests
        {
            [TestMethod]
            public void AggregatesWithNoDelimiter()
            {
                string[] enumerable = new[] { "1", "2", "3" };
                string aggregate = enumerable.ToStringAggregate();
                Assert.AreEqual("123", aggregate);
            }

            [TestMethod]
            public void AggregatesWithDelimiter()
            {
                string[] enumerable = new[] { "1", "2", "3" };
                string aggregate = enumerable.ToStringAggregate("#");
                Assert.AreEqual("1#2#3", aggregate);
            }

            [TestMethod]
            public void AggregatesWithSingleElementNoDelimiter()
            {
                string[] enumerable = new[] { "1" };
                string aggregate = enumerable.ToStringAggregate();
                Assert.AreEqual("1", aggregate);
            }

            [TestMethod]
            public void AggregatesWithSingleElementWithDelimiter()
            {
                string[] enumerable = new[] { "1" };
                string aggregate = enumerable.ToStringAggregate("asd");
                Assert.AreEqual("1", aggregate);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsOnNullCollection()
            {
                IEnumerable<string> nullCollection = null;
                string aggregate = nullCollection.ToStringAggregate("asd");
                Assert.AreEqual("", aggregate);
            }
        }
    }
}
