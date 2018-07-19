using BGC.Core;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Data.Relational.Mappings.MediaTypeInfoTypeMapperTests
{
    public class CtorTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullPropertyMapper()
        {
            Assert.Throws<ArgumentNullException>(() => new MediaTypeInfoTypeMapper(null, new MockDtoFactory()));
        }
    }

    internal class MediaTypeInfoPropertyMapperProxy : MediaTypeInfoPropertyMapper
    {
        protected override void CopyDataInternal(MediaTypeInfo source, MediaTypeInfoRelationalDto target)
        {
            CopyToDto_InvocationCount++;
            base.CopyDataInternal(source, target);
        }

        protected override void CopyDataInternal(MediaTypeInfoRelationalDto source, MediaTypeInfo target)
        {
            CopyToEntity_InvocationCount++;
            base.CopyDataInternal(source, target);
        }

        public int CopyToDto_InvocationCount { get; set; }

        public int CopyToEntity_InvocationCount { get; set; }
    }

    public class BuildDtoTests : TestFixtureBase
    {
        [Test]
        public void CallsPropertyMapperOnDtoCreation()
        {
            var propertyMapper = new MediaTypeInfoPropertyMapperProxy();
            var typeMapper = new MediaTypeInfoTypeMapper(propertyMapper, new MockDtoFactory());

            typeMapper.BuildDto(new MediaTypeInfo("image/jpeg"));

            Assert.AreEqual(1, propertyMapper.CopyToDto_InvocationCount);
        }
    }

    public class BuildTests : TestFixtureBase
    {
        [Test]
        public void CallsPropertyMapperOnEntityCreation()
        {
            var propertyMapper = new MediaTypeInfoPropertyMapperProxy();
            var typeMapper = new MediaTypeInfoTypeMapper(propertyMapper, new MockDtoFactory());

            typeMapper.Build(new MediaTypeInfoRelationalDto() { MimeType = "image/jpeg" });

            Assert.AreEqual(1, propertyMapper.CopyToEntity_InvocationCount);
        }
    }
}
