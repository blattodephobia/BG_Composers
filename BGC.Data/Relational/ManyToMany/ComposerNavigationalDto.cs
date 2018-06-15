using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.ManyToMany
{
    internal class ComposerNavigationalDto : ComposerRelationalDto, INavigationalDto
    {
        public IEnumerable<ArticleNavigationalDto> Articles { get; set; }

        public IEnumerable<NameRelationalDto> LocalizedNames { get; set; }
    }
}
