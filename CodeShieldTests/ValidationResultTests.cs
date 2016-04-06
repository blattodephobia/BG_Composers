using CodeShield;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShieldTests
{
    [TestClass]
    public class ValidationTests
    {
        [TestClass]
        public class CtorTests
        {
            [TestMethod]
            public void SetsSuccessToTrueIfExceptionProviderIsNull()
            {
                Validation<int> val = new Validation<int>(5, null);
                Assert.IsTrue(val.Success);
            }

            [TestMethod]
            public void SetsSuccessToFalseIfExceptionProviderIsNotNull()
            {
                Validation<int> val = new Validation<int>(5, x => new Exception());
                Assert.IsFalse(val.Success);
            }
        }

        [TestClass]
        public class AndTests
        {
            [TestMethod]
            public void SetsSuccessToTrueIfBothAreSucccessful()
            {
                Validation<int> val1 = new Validation<int>(3);
                Validation<int> val2 = new Validation<int>(5);
                Validation<int> result = val1.And(val2);
                Assert.IsTrue(result.Success);
            }

            [TestMethod]
            public void SetsSuccessToFalseIfEitherValidationFails1()
            {
                Validation<int> val1 = new Validation<int>(3, x => new Exception());
                Validation<int> val2 = new Validation<int>(5);
                Validation<int> result = val1.And(val2);
                Assert.IsFalse(result.Success);

            }

            [TestMethod]
            public void SetsSuccessToFalseIfEitherValidationFails2()
            {
                Validation<int> val1 = new Validation<int>(3);
                Validation<int> val2 = new Validation<int>(5, x => new Exception());
                Validation<int> result = val1.And(val2);
                Assert.IsFalse(result.Success);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void ThrowsFirstValidationsExceptionIfItFails1()
            {
                Validation<int> val1 = new Validation<int>(3, x => new ArgumentException());
                Validation<int> val2 = new Validation<int>(5, x => new IndexOutOfRangeException());
                Validation<int> result = val1.And(val2);
                result.ThrowOnError();
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void ThrowsFirstValidationsExceptionIfItFails2()
            {
                Validation<int> val1 = new Validation<int>(3, x => new ArgumentException());
                Validation<int> val2 = new Validation<int>(5);
                Validation<int> result = val1.And(val2);
                result.ThrowOnError();
            }

            [TestMethod]
            [ExpectedException(typeof(IndexOutOfRangeException))]
            public void ThrowsSecondValidationsExceptionIfOnlyItFails()
            {
                Validation<int> val1 = new Validation<int>(3);
                Validation<int> val2 = new Validation<int>(5, x => new IndexOutOfRangeException());
                Validation<int> result = val1.And(val2);
                result.ThrowOnError();
            }
        }
    }
}
