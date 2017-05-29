using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class ArticleManagementPermission : Permission, IArticleManagementPermission
    {
        public override string Name => nameof(IArticleManagementPermission);
    }
}
