using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities.Tests.LambdaCaptureFieldAccessorTests
{
    // this class is used instead of System.Tuple<,>, since on Mono platforms System.Tuple<,> is a dynamic type and these aren't supported
    internal class IndexerClass
    {
        public string Item1 { get; set; }

        public string Item2 { get; set; }

        private string _writeOnlyProperty;
        public string WriteOnlyProperty
        {
            set
            {
                _writeOnlyProperty = value;
            }
        }

        public string this[int itemIndex]
        {
            get
            {
                if (itemIndex < 1 || itemIndex > 2) throw new IndexOutOfRangeException();

                return itemIndex == 1 ? Item1 : Item2;
            }
        }

        public IndexerClass()
        {
        }

        public IndexerClass(string item1, string item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }

    [TestFixture]
    public class GetMemberValueTests
    {
        [Test]
        public void NoExceptionOnWriteOnlyProperty()
        {
            // this test uses a class with a write-only property; no exception should be thrown when constructing an accessor or getting a member
            IndexerClass testObj = new IndexerClass("item1", "item2");
            LambdaCaptureFieldAccessor<IndexerClass> accessor = new LambdaCaptureFieldAccessor<IndexerClass>();
            string item1 = accessor.GetMemberValue(testObj, nameof(IndexerClass.Item1)) as string;
            string item2 = accessor.GetMemberValue(testObj, nameof(IndexerClass.Item2)) as string;

            Assert.AreEqual(testObj.Item1, item1);
            Assert.AreEqual(testObj.Item2, item2);
        }

        [Test]
        public void GetValueOfReferenceType()
        {
            Tuple<string, string> testObj = new Tuple<string, string>("item1", "item2");
            LambdaCaptureFieldAccessor<Tuple<string, string>> accessor = new LambdaCaptureFieldAccessor<Tuple<string, string>>();
            string item1 = accessor.GetMemberValue(testObj, nameof(Tuple<string, string>.Item1)) as string;
            string item2 = accessor.GetMemberValue(testObj, nameof(Tuple<string, string>.Item2)) as string;

            Assert.AreEqual(testObj.Item1, item1);
            Assert.AreEqual(testObj.Item2, item2);
        }

        [Test]
        public void GetValueOfValueType()
        {
            Tuple<int, int> testObj = new Tuple<int, int>(1, 2);
            LambdaCaptureFieldAccessor<Tuple<int, int>> accessor = new LambdaCaptureFieldAccessor<Tuple<int, int>>();
            int item1 = (int)accessor.GetMemberValue(testObj, nameof(Tuple<string, string>.Item1));
            int item2 = (int)accessor.GetMemberValue(testObj, nameof(Tuple<string, string>.Item2));

            Assert.AreEqual(1, item1);
            Assert.AreEqual(2, item2);
        }

        [Test]
        public void ThrowsOnIncompatibleType()
        {
            Tuple<int> testObj = new Tuple<int>(1);
            LambdaCaptureFieldAccessor<Tuple<string>> accessor = new LambdaCaptureFieldAccessor<Tuple<string>>();
            Assert.Throws<InvalidOperationException>(() => accessor.GetMemberValue(testObj, nameof(Tuple<string>.Item1)));
        }

        [Test]
        public void ThrowsOnNull()
        {
            Tuple<string> testObj = new Tuple<string>("1");
            LambdaCaptureFieldAccessor<Tuple<string>> accessor = new LambdaCaptureFieldAccessor<Tuple<string>>();
            Assert.Throws<ArgumentNullException>(() => accessor.GetMemberValue(null, nameof(Tuple<string>.Item1)));
        }
    }
}
