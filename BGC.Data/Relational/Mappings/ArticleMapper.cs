using BGC.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal class ArticleMapper : RelationalMapper<ComposerArticle, ArticleRelationalDto>
    {
        private readonly ComposerMapper _composerMapper;

        public override ArticleRelationalDto CopyData(ComposerArticle source, ArticleRelationalDto target)
        {
            target.CreatedUtc = source.CreatedUtc;
            target.Id = source.Id;
            target.IsArchived = source.IsArchived;
            target.Language = source.Language.Name;
            target.StorageId = source.StorageId;

            return target;
        }

        public override ComposerArticle ToEntity(ArticleRelationalDto dto)
        {
            ComposerArticle result = new ComposerArticle(_composerMapper.ToEntity(dto.Composer), null, CultureInfo.GetCultureInfo(dto.Language));
            result.CreatedUtc = dto.CreatedUtc;
            result.Id = dto.Id;
            result.IsArchived = dto.IsArchived;
            result.StorageId = dto.StorageId;

            return result;
        }

        public ArticleMapper(ComposerMapper composerMapper)
        {
            _composerMapper = composerMapper;
        }
    }
}
