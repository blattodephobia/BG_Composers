using BGC.Core.Exceptions;
using BGC.Data.Relational;
using BGC.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Data.Utilities.DtoUtilsTests
{
    public class GetIdentityPropertyTests : TestFixtureBase
    {
        [Identity(nameof(Key))]
        internal class IdAttrDto
        {
            public Guid Key { get; set; }

            public long Id { get; set; }
            
            public int False_Id { get; set; }
        }
        
        internal class NoIdAttrDto
        {
            public Guid Key { get; set; }

            public long Id { get; set; }
            
            public int False_Id { get; set; }
        }
        
        [Test]
        public void GetsIdWithIdentityAttribute()
        {
            PropertyInfo idProp = DtoUtils.GetIdentityProperty<IdAttrDto>();
            Assert.AreEqual(nameof(IdAttrDto.Key), idProp.Name);
        }

        [Test]
        public void ThrowsExceptionIfNoIdentityAttr()
        {
            Assert.Throws<MissingMemberException>(() => DtoUtils.GetIdentityProperty<NoIdAttrDto>());
        }
    }

    public class GetKeyPropertiesTests : TestFixtureBase
    {
        internal class MultiKeyAttrDto
        {
            public Guid Key { get; set; }

            public long Id { get; set; }

            [Key]
            public int ActualKey1 { get; set; }

            [Key]
            public int ActualKey2 { get; set; }
        }

        internal class KeyAttrDto
        {
            public Guid Key { get; set; }

            [Key]
            public long ActualKey { get; set; }
        }

        internal class IdPropertyDto
        {
            public int Id { get; set; }

            public long Key { get; set; }
        }

        internal class NoIdDto
        {
            public int Key { get; set; }

            public Guid OtherProperty { get; set; }
        }

        [Identity(nameof(Id))]
        internal class DuplicateIdButConsistentDto
        {
            [Key]
            public int Id { get; set; }
        }

        [Test]
        public void GetsIdWithKeyAttribute()
        {
            PropertyInfo[] idProps = DtoUtils.GetKeyProperties<KeyAttrDto>();
            Assert.AreEqual(nameof(KeyAttrDto.ActualKey), idProps[0].Name);
        }

        [Test]
        public void GetsIdWithIdProperty()
        {
            PropertyInfo[] idProps = DtoUtils.GetKeyProperties<IdPropertyDto>();
            Assert.AreEqual(nameof(IdPropertyDto.Id), idProps[0].Name);
        }

        [Test]
        public void GetsIdsWithMultipleKeys()
        {
            PropertyInfo[] idProps = DtoUtils.GetKeyProperties<MultiKeyAttrDto>();

            string idPropertyName1 = nameof(MultiKeyAttrDto.ActualKey1);
            Assert.IsTrue(idPropertyName1 == idProps.FirstOrDefault(p => p.Name == idPropertyName1)?.Name);

            string idPropertyName2 = nameof(MultiKeyAttrDto.ActualKey2);
            Assert.IsTrue(idPropertyName2 == idProps.FirstOrDefault(p => p.Name == idPropertyName2)?.Name);
        }

        [Test]
        public void GetsIdWithDuplicateButConsistentDto()
        {
            PropertyInfo[] ids = DtoUtils.GetKeyProperties<DuplicateIdButConsistentDto>();
            Assert.AreEqual(nameof(DuplicateIdButConsistentDto.Id), ids[0].Name);
        }

        [Test]
        public void ThrowsExceptionIfNoSuitableProperty()
        {
            Assert.Throws<MissingMemberException>(() => DtoUtils.GetKeyProperties<NoIdDto>());
        }
    }
}
