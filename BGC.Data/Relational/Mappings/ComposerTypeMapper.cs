using BGC.Core;
using BGC.Utilities;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal class ComposerTypeMapper : DomainTypeMapperBase<Composer, ComposerRelationalDto>
    {
        private readonly ComposerPropertyMapper _composerMapper;
        private readonly NamePropertyMapper _nameMapper;
        private readonly ArticlePropertyMapper _articleMapper;
        private readonly MediaTypeInfoPropertyMapper _mediaTypeInfoMapper;

        private static void SyncCollection<T>(IEnumerable<T> source, ICollection<T> destination)
        {
            HashSet<T> _source = (source as HashSet<T>) ?? new HashSet<T>(source);

            foreach (T item in source)
            {
                if (!destination.Contains(item))
                {
                    destination.Add(item);
                }
            }

            T[] missingItems = destination.Where(t => !_source.Contains(t)).ToArray();
            for (int i = 0; i < missingItems.Length; i++)
            {
                destination.Remove(missingItems[i]);
            }
        }

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

        private MediaTypeInfoRelationalDto GetMediaDto(MediaTypeInfo entity) => DtoFactory.GetDtoFor(entity, _mediaTypeInfoMapper);

        private void UpdateMedia(Composer entity, ComposerRelationalDto composer)
        {
            var mediaDtos = from media in entity.Profile.Media
                            let mediaDto = _mediaTypeInfoMapper.CopyData(media, GetMediaDto(media))
                            select DtoFactory.GetIntermediateDto(composer, mediaDto) as ComposerMediaRelationalDto;
            SyncCollection(mediaDtos, composer.Media);

            if (entity.Profile.ProfilePicture != null)
            {
                composer.ProfilePicture = _mediaTypeInfoMapper.CopyData(entity.Profile.ProfilePicture, GetMediaDto(entity.Profile.ProfilePicture));
            }
        }

        private void UpdateArticles(Composer entity, ComposerRelationalDto composer)
        {
            var articleDtos = from article in entity.GetArticles(includeArchived: true)
                              let articleDto = GetArticleDto(composer, article)
                              select _articleMapper.CopyData(article, articleDto);
            composer.Articles.AddMissingRange(articleDtos);
        }

        private void UpdateNames(Composer entity, ComposerRelationalDto composer)
        {
            var nameDtos = from nameKvp in entity.Name.All()
                           let nameDto = GetNameDto(composer, nameKvp.Value)
                           select _nameMapper.CopyData(nameKvp.Value, nameDto);
            SyncCollection(nameDtos, composer.LocalizedNames);
        }

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
                HashSet<Guid> presentStorageIds = new HashSet<Guid>(entity.Profile?.Media.Select(m => m.StorageId) ?? Enumerable.Empty<Guid>());
                var mediaDtos = from media in entity.Profile.Media
                                where !presentStorageIds.Contains(media.StorageId)
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

        protected override ComposerRelationalDto BuildDtoInternal(Composer entity)
        {
            ComposerRelationalDto composer = DtoFactory.GetDtoFor(entity, _composerMapper);
            _composerMapper.CopyData(entity, composer);

            UpdateNames(entity, composer);
            UpdateArticles(entity, composer);

            if (entity.Profile != null)
            {
                UpdateMedia(entity, composer);
            }

            return composer;
        }

        protected override Composer BuildInternal(ComposerRelationalDto dto)
        {
            Composer result = _composerMapper.CopyData(dto, new Composer());

            foreach (NameRelationalDto name in dto.LocalizedNames)
            {
                ComposerName domainName = _nameMapper.CopyData(name, new ComposerName(name.FullName, name.Language));
                domainName.Composer = result;
                result.Name[domainName.Language] = domainName;
            }

            foreach (ArticleRelationalDto article in dto.Articles)
            {
                var culture = CultureInfo.GetCultureInfo(article.Language);
                result.AddArticle(_articleMapper.CopyData(article, new ComposerArticle(result, result.Name[culture], culture)));
            }

            foreach (ComposerMediaRelationalDto media in dto.Media)
            {
                result.Profile.Media.Add(_mediaTypeInfoMapper.CopyData(media.MediaEntry, new MediaTypeInfo(media.MediaEntry.MimeType)));
            }

            return result;
        }

        public ComposerTypeMapper(ComposerMappers mappers, IDtoFactory dtoFactory) :
            base(dtoFactory)
        {
            Shield.ArgumentNotNull(mappers).ThrowOnError();

            _composerMapper = mappers.ComposerMapper;
            _nameMapper = mappers.NameMapper;
            _articleMapper = mappers.ArticleMapper;
            _mediaTypeInfoMapper = mappers.MediaTypeInfoMapper;
        }
    }
}
