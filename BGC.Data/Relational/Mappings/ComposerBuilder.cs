using BGC.Core;
using BGC.Data.Relational.ManyToMany;
using System;
using System.Collections.Generic;
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

        public ComposerBuilder(ComposerMapper composerMapper, NameMapper nameMapper, ArticleMapper articleMapper, ProfileMapper profileMapper)
        {
            _composerMapper = composerMapper;
            _nameMapper = nameMapper;
            _articleMapper = articleMapper;
            _profileMapper = profileMapper;
        }

        public override Composer Build(ComposerNavigationalDto dto)
        {
            Composer result = _composerMapper.ToEntity(dto);
            foreach (ArticleNavigationalDto article in dto.Articles)
            {
                result.AddArticle(_articleMapper.ToEntity(article));
            }

            if (dto.Profile != null)
            {
                result.Profile = _profileMapper.ToEntity(dto.Profile);
            }

            foreach (NameRelationalDto name in dto.LocalizedNames)
            {
                ComposerName domainName = _nameMapper.ToEntity(name);
                result.Name[domainName.Language] = domainName;
            }

            return result;
        }
    }
}
