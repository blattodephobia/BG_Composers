using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal class ComposerBreakdown : DomainBreakdownBase<Composer>
    {
        private readonly ComposerMapper _composerMapper;
        private readonly NameMapper _nameMapper;
        private readonly ArticleMapper _articleMapper;
        private readonly ProfileMapper _profileMapper;

        public override IEnumerable<RelationdalDtoBase> Breakdown(Composer entity)
        {
            List<RelationdalDtoBase> dtos = new List<RelationdalDtoBase>();
            ComposerRelationalDto composer = new ComposerRelationalDto();
            _composerMapper.CopyData(entity, composer);
            dtos.Add(composer);

            var nameDtos = from nameKvp in entity.Name.All()
                           let dto = _nameMapper.CopyData(nameKvp.Value, new NameRelationalDto())
                           select dto;
            dtos.AddRange(nameDtos);

            var articleDtos = from article in entity.GetArticles(includeArchived: true)
                              let dto = _articleMapper.CopyData(article, new ArticleRelationalDto())
                              select dto;
            dtos.AddRange(articleDtos);

            if (entity.Profile != null)
            {
                ProfileRelationalDto profile = _profileMapper.CopyData(entity.Profile, new ProfileRelationalDto());
                dtos.Add(profile);
            }

            return dtos;
        }

        public ComposerBreakdown(ComposerMapper composerMapper, NameMapper nameMapper, ArticleMapper articleMapper, ProfileMapper profileMapper)
        {
            _composerMapper = composerMapper;
            _nameMapper = nameMapper;
            _articleMapper = articleMapper;
            _profileMapper = profileMapper;
        }
    }
}
