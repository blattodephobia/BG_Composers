using BGC.Core;
using BGC.Core.Models;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal class GlossaryEntryTypeMapper : DomainTypeMapperBase<GlossaryEntry, GlossaryEntryRelationalDto>
    {
        private readonly GlossaryEntryPropertyMapper _glossaryPropertyMapper;

        protected override IEnumerable<RelationdalDtoBase> BreakdownInternal(GlossaryEntry entity)
        {
            throw new NotImplementedException();
        }

        protected override GlossaryEntryRelationalDto BuildDtoInternal(GlossaryEntry entity)
        {
            GlossaryEntryRelationalDto result = DtoFactory.GetDtoFor(entity, _glossaryPropertyMapper);
            return result;
        }

        protected override GlossaryEntry BuildInternal(GlossaryEntryRelationalDto dto)
        {
            var result = new GlossaryEntry();
            _glossaryPropertyMapper.CopyData(dto, result);
            foreach (GlossaryDefinitionRelationalDto definition in dto.Translations)
            {
                var entityDef = new GlossaryDefinition(CultureInfo.GetCultureInfo(definition.Language), definition.DefinitionTranslation, definition.TermTranslation);
                result.Definitions.Add(entityDef);
            }

            return result;
        }

        public GlossaryEntryTypeMapper(GlossaryEntryPropertyMapper glossaryPropertyMapper, IDtoFactory dtoFactory) :
            base(dtoFactory)
        {
            _glossaryPropertyMapper = glossaryPropertyMapper;
        }
    }
}
