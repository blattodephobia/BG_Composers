using BGC.Core;
using BGC.Data.Relational.Mappings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Repositories
{
    internal class MediaTypeInfoRepository : EntityFrameworkRepository<Guid, MediaTypeInfo, MediaTypeInfoRelationalDto>, IMediaTypeInfoRepository
    {
        public MediaTypeInfoRepository(MediaTypeInfoTypeMapper typeMapper, DbContext context) : base(typeMapper, context)
        {
        }
    }
}
