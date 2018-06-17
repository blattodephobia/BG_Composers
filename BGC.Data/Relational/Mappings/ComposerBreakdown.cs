using BGC.Core;
using CodeShield;
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
        private readonly MediaTypeInfoMapper _mediaTypeInfoMapper;

        protected override IEnumerable<RelationdalDtoBase> BreakdownInternal(Composer entity)
        {
            List<RelationdalDtoBase> dtos = new List<RelationdalDtoBase>();
            ComposerRelationalDto composer = new ComposerRelationalDto();
            _composerMapper.CopyData(entity, composer);
            dtos.Add(composer);

            var nameDtos = from nameKvp in entity.Name.All()
                           let dto = _nameMapper.CopyData(nameKvp.Value, new NameRelationalDto() { Composer = composer })
                           select dto;
            dtos.AddRange(nameDtos);

            var articleDtos = from article in entity.GetArticles(includeArchived: true)
                              let dto = _articleMapper.CopyData(article, new ArticleRelationalDto() { Composer = composer })
                              select dto;
            dtos.AddRange(articleDtos);
            
            ProfileRelationalDto profile = _profileMapper.CopyData(entity.Profile);
            if (profile != null)
            {
                IEnumerable<MediaTypeInfoRelationalDto> media = entity.Profile?.Media.Select(m => _mediaTypeInfoMapper.CopyData(m, new MediaTypeInfoRelationalDto()));

                dtos.Add(_mediaTypeInfoMapper.CopyData(entity.Profile.ProfilePicture, new MediaTypeInfoRelationalDto()));
                dtos.Add(profile);
                dtos.AddRange(media);
            }

            return dtos;
        }

        public ComposerBreakdown(ComposerMappers mappers)
        {
            Shield.ArgumentNotNull(mappers).ThrowOnError();

            _composerMapper = mappers.ComposerMapper;
            _nameMapper = mappers.NameMapper;
            _articleMapper = mappers.ArticleMapper;
            _profileMapper = mappers.ProfileMapper;
            _mediaTypeInfoMapper = mappers.MediaTypeInfoMapper;
        }
    }
}
