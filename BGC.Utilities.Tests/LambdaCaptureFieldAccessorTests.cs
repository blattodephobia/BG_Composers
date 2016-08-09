using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities.Tests
{
        [TestFixture]
        public class GetMemberValueTests
        {
            [Test]
            public void GetValueOfReferenceType()
            {
                Tuple<string, string> testObj = new Tuple<string, string>("item1", "item2");
                LambdaCaptureFieldAccessor<Tuple<string, string>> accessor = new LambdaCaptureFieldAccessor<Tuple<string, string>>();
                string item1 = accessor.GetMemberValue(testObj, nameof(Tuple<string, string>.Item1)) as string;
                string item2 = accessor.GetMemberValue(testObj, nameof(Tuple<string, string>.Item2)) as string;

                Assert.AreEqual("item1", item1);
                Assert.AreEqual("item2", item2);
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
