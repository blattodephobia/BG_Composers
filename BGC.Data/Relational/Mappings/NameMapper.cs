using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal class NameMapper : RelationalMapper<ComposerName, NameRelationalDto>
    {
        private readonly ComposerMapper _composerMapper;

        public override NameRelationalDto CopyData(ComposerName source, NameRelationalDto target)
        {
            target.FullName = source.FullName;
            target.Id = source.Id;
            target.Language = source.Language.Name;

            target.Composer = target.Composer ?? new ComposerRelationalDto();
            _composerMapper.CopyData(source.Composer, target.Composer);

            return target;
        }

        public override ComposerName ToEntity(NameRelationalDto dto)
        {
            ComposerName result = new ComposerName(dto.FullName, dto.Language)
            {
                Id = dto.Id,
                Composer = _composerMapper.ToEntity(dto.Composer)
            };

            return result;
        }

        public NameMapper(ComposerMapper composerMapper)
        {
            _composerMapper = composerMapper;
        }
    }
}
