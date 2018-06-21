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

        private NameRelationalDto GetNameDto(ComposerRelationalDto principal, ComposerName entity)
        {
            NameRelationalDto result = DtoFactory.GetDtoFor(entity, _nameMapper);
            if (result.Composer == null)
            {
                result.Composer = principal;
            }

            return result;
        }

        private ArticleRelationalDto GetArticleDto(ComposerRelationalDto principal, ComposerArticle entity)
        {
            ArticleRelationalDto result = DtoFactory.GetDtoFor(entity, _articleMapper);
            if (result.Composer == null)
            {
                result.Composer = principal;
            }

            return result;
        }

        private ProfileRelationalDto GetProfileDto(ComposerRelationalDto principal, ComposerProfile entity)
        {
            ProfileRelationalDto result = DtoFactory.GetDtoFor(entity, _profileMapper);
            if (result.Composer == null)
            {
                result.Composer = principal;
            }

            return result;
        }

        private MediaTypeInfoRelationalDto GetMediaDto(MediaTypeInfo entity) => DtoFactory.GetDtoFor(entity, _mediaTypeInfoMapper);

        protected override IEnumerable<RelationdalDtoBase> BreakdownInternal(Composer entity)
        {
            List<RelationdalDtoBase> dtos = new List<RelationdalDtoBase>();
            ComposerRelationalDto composer = DtoFactory.GetDtoFor(entity, _composerMapper);
            _composerMapper.CopyData(entity, composer);
            dtos.Add(composer);

            var nameDtos = from nameKvp in entity.Name.All()
                           let nameDto = GetNameDto(composer, nameKvp.Value)
                           select _nameMapper.CopyData(nameKvp.Value, nameDto);
            dtos.AddRange(nameDtos);

            var articleDtos = from article in entity.GetArticles(includeArchived: true)
                              let articleDto = GetArticleDto(composer, article)
                              select _articleMapper.CopyData(article, articleDto);
            dtos.AddRange(articleDtos);

            if (entity.Profile != null)
            {
                ProfileRelationalDto profile = _profileMapper.CopyData(entity.Profile, GetProfileDto(composer, entity.Profile));
                dtos.Add(profile);

                var mediaDtos = from media in entity.Profile.Media
                                let mediaDto = _mediaTypeInfoMapper.CopyData(media, GetMediaDto(media))
                                select mediaDto;

                dtos.AddRange(mediaDtos);

                if (entity.Profile.ProfilePicture != null)
                {
                    dtos.Add(_mediaTypeInfoMapper.CopyData(entity.Profile.ProfilePicture, GetMediaDto(entity.Profile.ProfilePicture)));
                }
            }

            return dtos;
        }

        public ComposerBreakdown(ComposerMappers mappers, IDtoFactory dtoFactory) :
            base(dtoFactory)
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
