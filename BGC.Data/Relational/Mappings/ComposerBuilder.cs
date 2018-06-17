using BGC.Core;
using BGC.Data.Relational.ManyToMany;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal class ComposerBuilder : DomainBuilderBase<ComposerNavigationalDto, Composer>
    {
        private readonly ComposerMapper _composerMapper;
        private readonly NameMapper _nameMapper;
        private readonly ArticleMapper _articleMapper;
        private readonly ProfileMapper _profileMapper;
        private readonly MediaTypeInfoMapper _mediaMapper;

        public ComposerBuilder(ComposerMappers mappers) :
            this(mappers.ComposerMapper, mappers.NameMapper, mappers.ArticleMapper, mappers.ProfileMapper, mappers.MediaTypeInfoMapper)
        {

        }

        public ComposerBuilder(ComposerMapper composerMapper, NameMapper nameMapper, ArticleMapper articleMapper, ProfileMapper profileMapper, MediaTypeInfoMapper mediaMapper)
        {
            _composerMapper = composerMapper;
            _nameMapper = nameMapper;
            _articleMapper = articleMapper;
            _profileMapper = profileMapper;
            _mediaMapper = mediaMapper;
        }

        protected override Composer BuildInternal(ComposerNavigationalDto dto)
        {
            Composer result = _composerMapper.CopyData(dto, new Composer());

            foreach (NameRelationalDto name in dto.LocalizedNames)
            {
                ComposerName domainName = _nameMapper.CopyData(name, new ComposerName(name.FullName, name.Language));
                result.Name[domainName.Language] = domainName;
            }

            foreach (ArticleNavigationalDto article in dto.Articles)
            {
                var culture = CultureInfo.GetCultureInfo(article.Language);
                result.AddArticle(_articleMapper.CopyData(article, new ComposerArticle(result, result.Name[culture], culture)));
            }

            if (dto.Profile != null)
            {
                result.Profile = _profileMapper.CopyData(dto.Profile, new ComposerProfile());
                result.Profile.ProfilePicture = _mediaMapper.CopyData(dto.Profile.ProfilePicture, new MediaTypeInfo(dto.Profile.ProfilePicture.MimeType));
            }

            return result;
        }
    }
}
