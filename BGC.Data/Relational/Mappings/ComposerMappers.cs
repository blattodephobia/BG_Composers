using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal class ComposerMappers
    {
        public virtual ComposerMapper ComposerMapper { get; private set; }
        public virtual NameMapper NameMapper { get; private set; }
        public virtual ArticleMapper ArticleMapper { get; private set; }
        public virtual ProfileMapper ProfileMapper { get; private set; }
        public virtual MediaTypeInfoMapper MediaTypeInfoMapper { get; private set; }

        public ComposerMappers() :
            this(new ComposerMapper(), new NameMapper(), new ArticleMapper(), new ProfileMapper(), new MediaTypeInfoMapper())
        {
        }

        public ComposerMappers(ComposerMapper composerMapper, NameMapper nameMapper, ArticleMapper articleMapper, ProfileMapper profileMapper, MediaTypeInfoMapper mediaTypeInfoMapper)
        {
            ComposerMapper = composerMapper;
            NameMapper = nameMapper;
            ArticleMapper = articleMapper;
            ProfileMapper = profileMapper;
            MediaTypeInfoMapper = mediaTypeInfoMapper;
        }
    }
}
