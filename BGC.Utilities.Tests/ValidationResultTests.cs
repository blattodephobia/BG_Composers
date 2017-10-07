using CodeShield;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShieldTests
{
    [TestFixture]
    public class CtorTests
    {
        [Test]
        public void SetsSuccessToTrueIfExceptionProviderIsNull()
        {
            Validation<int> val = new Validation<int>(5, null);
            Assert.IsTrue(val.Success);
        }

        [Test]
        public void SetsSuccessToFalseIfExceptionProviderIsNotNull()
        {
            Validation<int> val = new Validation<int>(5, x => new Exception());
            Assert.IsFalse(val.Success);
        }
    }

    [TestFixture]
    public class AndTests
    {
        [Test]
        public void SetsSuccessToTrueIfBothAreSucccessful()
        {
            Validation<int> val1 = new Validation<int>(3);
            Validation<int> val2 = new Validation<int>(5);
            Validation<int> result = val1.And(val2);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void SetsSuccessToFalseIfEitherValidationFails1()
        {
            Validation<int> val1 = new Validation<int>(3, x => new Exception());
            Validation<int> val2 = new Validation<int>(5);
            Validation<int> result = val1.And(val2);
            Assert.IsFalse(result.Success);

        }

        [Test]
        public void SetsSuccessToFalseIfEitherValidationFails2()
        {
            Validation<int> val1 = new Validation<int>(3);
            Validation<int> val2 = new Validation<int>(5, x => new Exception());
            Validation<int> result = val1.And(val2);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ThrowsFirstValidationsExceptionIfItFails1()
        {
            Validation<int> val1 = new Validation<int>(3, x => new ArgumentException());
            Validation<int> val2 = new Validation<int>(5, x => new IndexOutOfRangeException());
            Validation<int> result = val1.And(val2);
            Assert.Throws<ArgumentException>(() => result.ThrowOnError());
        }

        [Test]
        public void ThrowsFirstValidationsExceptionIfItFails2()
        {
            Validation<int> val1 = new Validation<int>(3, x => new ArgumentException());
            Validation<int> val2 = new Validation<int>(5);
            Validation<int> result = val1.And(val2);
            Assert.Throws<ArgumentException>(() => result.ThrowOnError());
        }

        [Test]
        public void ThrowsSecondValidationsExceptionIfOnlyItFails()
        {
            Validation<int> val1 = new Validation<int>(3);
            Validation<int> val2 = new Validation<int>(5, x => new IndexOutOfRangeException());
            Assert.Throws<IndexOutOfRangeException>(() => val1.And(val2).ThrowOnError());
        }
    }

}
