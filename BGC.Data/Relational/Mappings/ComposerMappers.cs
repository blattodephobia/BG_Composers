using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal class ComposerMappers
    {
        public virtual ComposerPropertyMapper ComposerMapper { get; private set; }
        public virtual NamePropertyMapper NameMapper { get; private set; }
        public virtual ArticlePropertyMapper ArticleMapper { get; private set; }
        public virtual MediaTypeInfoPropertyMapper MediaTypeInfoMapper { get; private set; }

        public ComposerMappers() :
            this(new ComposerPropertyMapper(), new NamePropertyMapper(), new ArticlePropertyMapper(), new MediaTypeInfoPropertyMapper())
        {
        }

        public ComposerMappers(ComposerPropertyMapper composerMapper, NamePropertyMapper nameMapper, ArticlePropertyMapper articleMapper, MediaTypeInfoPropertyMapper mediaTypeInfoMapper)
        {
            ComposerMapper = composerMapper;
            NameMapper = nameMapper;
            ArticleMapper = articleMapper;
            MediaTypeInfoMapper = mediaTypeInfoMapper;
        }
    }
}
