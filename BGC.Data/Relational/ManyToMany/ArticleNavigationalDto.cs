using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.ManyToMany
{
    internal class ArticleNavigationalDto : ArticleRelationalDto, INavigationalDto
    {
        public IEnumerable<MediaTypeInfoRelationalDto> Media { get; set; }
    }
}
