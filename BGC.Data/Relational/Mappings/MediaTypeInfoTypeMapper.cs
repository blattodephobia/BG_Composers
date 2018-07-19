using BGC.Core;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal class MediaTypeInfoTypeMapper : DomainTypeMapperBase<MediaTypeInfo, MediaTypeInfoRelationalDto>
    {
        private readonly MediaTypeInfoPropertyMapper _propertyMapper;

        protected override IEnumerable<RelationdalDtoBase> BreakdownInternal(MediaTypeInfo entity)
        {
            throw new NotImplementedException();
        }

        protected override MediaTypeInfoRelationalDto BuildDtoInternal(MediaTypeInfo entity)
        {
            MediaTypeInfoRelationalDto dto = DtoFactory.GetDtoFor(entity, _propertyMapper);
            _propertyMapper.CopyData(entity, dto);

            return dto;
        }

        protected override MediaTypeInfo BuildInternal(MediaTypeInfoRelationalDto dto)
        {
            MediaTypeInfo result = new MediaTypeInfo(dto.OriginalFileName, dto.MimeType);

            _propertyMapper.CopyData(dto, result);

            return result;
        }

        public MediaTypeInfoTypeMapper(MediaTypeInfoPropertyMapper propertyMapper, IDtoFactory dtoFactory) :
            base(dtoFactory)
        {
            Shield.ArgumentNotNull(propertyMapper).ThrowOnError();

            _propertyMapper = propertyMapper;
        }
    }
}
