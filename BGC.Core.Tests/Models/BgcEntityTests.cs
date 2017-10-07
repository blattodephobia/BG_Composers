using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models.BgcEntityTests
{
    [TestFixture]
    public class EnsureValidTests
    {
        public class TestEntity : BgcEntity<long>
        {
            [Required]
            public virtual string RequiredProperty
            {
                set
                {
                    EnsureValid(value);
                }
            }
        }

        public class InheritingEntity : TestEntity
        {
            [MaxLength(2)]
            public override string RequiredProperty
            {
                set
                {
                    EnsureValid(value);
                }
            }
        }

        [Test]
        public void ThrowsExceptionIfInvalidValue()
        {
            var entity = new TestEntity();
            Assert.Throws<ValidationException>(() => entity.RequiredProperty = null);
        }

        [Test]
        public void NoExceptionIfValidValue()
        {
            var entity = new TestEntity();
            entity.RequiredProperty = "sample";
        }

        [Test]
        public void ThrowsExceptionIfInvalidValue_Inheriting1()
        {
            var entity = new InheritingEntity();
            Assert.Throws<ValidationException>(() => entity.RequiredProperty = null);
        }

        [Test]
        public void ThrowsExceptionIfInvalidValue_Inheriting2()
        {
            var entity = new InheritingEntity();
            ValidationException exception = Assert.Throws<ValidationException>(() => entity.RequiredProperty = "sample");
            Assert.IsTrue(exception.Data.Contains(typeof(MaxLengthAttribute).FullName));
        }
    }
}
