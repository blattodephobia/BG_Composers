using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data
{
#pragma warning disable S3246 // S3246: Generic type parameters should be co/contravariant when possible
    /* This is disabled, because it incorrectly assumes TKey can be contravariant.
     * TKey is a struct and cannot have more derived classes. */
    public interface INonQueryableRepository<TKey, TEntity> : IDbPersist
#pragma warning restore S3246
        where TKey : struct
        where TEntity : BgcEntity<TKey>
    {
        TEntity Find(TKey domainKey);

        void AddOrUpdate(TEntity entity);

        void Delete(params TKey[] keys);
    }
}
