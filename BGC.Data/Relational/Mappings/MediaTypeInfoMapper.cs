using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal class MediaTypeInfoMapper : RelationalMapper<MediaTypeInfo, MediaTypeInfoRelationalDto>
    {
        protected override void CopyDataInternal(MediaTypeInfo source, MediaTypeInfoRelationalDto target)
        {
            target.ExternalLocation = source.ExternalLocation;
            target.StorageId = source.StorageId;
            target.MimeType = source.MimeType.Name;
            target.OriginalFileName = source.OriginalFileName;
        }

        protected override void CopyDataInternal(MediaTypeInfoRelationalDto source, MediaTypeInfo target)
        {
            target.ExternalLocation = source.ExternalLocation;
            target.StorageId = source.StorageId;
            target.OriginalFileName = source.OriginalFileName;
            target.MimeType = new ContentType(source.MimeType);
        }
    }
}
