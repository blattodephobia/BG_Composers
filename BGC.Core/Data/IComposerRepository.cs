using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data
{
    public interface IComposerRepository : INonQueryableRepository<Guid, Composer>
    {
        IEnumerable<Composer> Find(Expression<Func<IComposerNameDto, bool>> selector);
    }
}
