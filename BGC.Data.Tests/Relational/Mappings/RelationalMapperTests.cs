using BGC.Core;
using BGC.Data.Relational.Mappings;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Data.Relational.Mappings.RelationalMapperTests
{
    public class CopyDataTests : TestFixtureBase
    {
        private class MapperImpl : RelationalMapper<MediaTypeInfo, MediaTypeInfoRelationalDto>
        {
            protected override void CopyDataInternal(MediaTypeInfo source, MediaTypeInfoRelationalDto target)
            {
                CopyToDtoCallback?.Invoke(source, target);
            }

            protected override void CopyDataInternal(MediaTypeInfoRelationalDto source, MediaTypeInfo target)
            {
                CopyToEntityCallback?.Invoke(source, target);
            }

            public Action<MediaTypeInfo, MediaTypeInfoRelationalDto> CopyToDtoCallback { get; set; }

            public Action<MediaTypeInfoRelationalDto, MediaTypeInfo> CopyToEntityCallback { get; set; }
        }

        [Test]
        public void DoesntCopyIfNullSource()
        {
            var mapper = new Mock<RelationalMapper<MediaTypeInfo, MediaTypeInfoRelationalDto>>();
            MediaTypeInfoRelationalDto target = new MediaTypeInfoRelationalDto();
            mapper.Object.CopyData(null, target);

            Assert.AreEqual(default(string), target.ExternalLocation);
            Assert.AreEqual(default(int), target.Id);
            Assert.AreEqual(default(string), target.MimeType);
            Assert.AreEqual(default(string), target.OriginalFileName);
            Assert.AreEqual(default(Guid), target.StorageId);
        }
        
        [Test]
        public void ThrowsExceptionIfNullEntityTarget()
        {
            Assert.Throws<ArgumentNullException>(() => new MapperImpl().CopyData(new MediaTypeInfoRelationalDto(), null));
        }
    }
}
