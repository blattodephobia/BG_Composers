using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Globalization;
using BGC.Core.Models;

namespace BGC.Data.Relational.Mappings
{
    internal class GlossaryDefinitionPropertyMapper : RelationalPropertyMapper<GlossaryDefinition, GlossaryDefinitionRelationalDto>
    {
        protected override void CopyDataInternal(GlossaryDefinition source, GlossaryDefinitionRelationalDto target)
        {
            target.Language = source.Language.Name;
            target.TermTranslation = source.Term;
            target.DefinitionTranslation = source.Definition;
        }

        protected override void CopyDataInternal(GlossaryDefinitionRelationalDto source, GlossaryDefinition target)
        {
            target.Language = source.Language.ToCultureInfo();
            target.Term = source.TermTranslation;
            target.Definition = source.DefinitionTranslation;
        }

        protected override Expression<Func<GlossaryDefinitionRelationalDto, bool>> GetComparisonInternal(GlossaryDefinition entity)
        {
            return (GlossaryDefinitionRelationalDto dto) =>
                dto.TermTranslation == entity.Term &&
                dto.DefinitionTranslation == entity.Definition &&
                dto.Language == entity.Language.Name;
        }
    }
}
