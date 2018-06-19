using BGC.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace BGC.Data.Relational.Mappings
{
    internal class ArticleMapper : RelationalMapperBase<ComposerArticle, ArticleRelationalDto>
    {
        private readonly ComposerMapper _composerMapper;

        protected override Expression<Func<ArticleRelationalDto, bool>> GetComparisonInternal(ComposerArticle entity)
        {
            Expression<Func<ArticleRelationalDto, bool>> result = (dto) => dto.StorageId == entity.StorageId;

            return result;
        }

        protected override void CopyDataInternal(ComposerArticle source, ArticleRelationalDto target)
        {
            target.CreatedUtc = source.CreatedUtc;
            target.Id = source.Id;
            target.IsArchived = source.IsArchived;
            target.Language = source.Language.Name;
            target.StorageId = source.StorageId;
        }

        protected override void CopyDataInternal(ArticleRelationalDto source, ComposerArticle target)
        {
            target.CreatedUtc = source.CreatedUtc;
            target.Id = source.Id;
            target.IsArchived = source.IsArchived;
            target.StorageId = source.StorageId;
            target.Language = CultureInfo.GetCultureInfo(source.Language);
        }
    }
}
