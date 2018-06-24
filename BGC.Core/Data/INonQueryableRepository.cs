using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data
{
    public interface INonQueryableRepository<TKey, TEntity>
        where TKey : struct
        where TEntity : BgcEntity<TKey>
    {
        TEntity Find(TKey key);

        void AddOrUpdate(TEntity entity);

        void Delete(params TKey[] keys);
    }
}
