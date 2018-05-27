using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public static class RepositoryExtensions
    {
        public static void Delete<TKey, TEntity>(this INonQueryableRepository<TKey, TEntity> repo, IEnumerable<TEntity> entities)
            where TKey : struct
            where TEntity : BgcEntity<TKey>
        {
            Shield.ArgumentNotNull(repo).ThrowOnError();
            Shield.ArgumentNotNull(entities).ThrowOnError();

            if (entities.Any())
            {
                repo.Delete(entities.Select(e => e.Id).ToArray());
            }
        }

        public static void Delete<TKey, TEntity>(this INonQueryableRepository<TKey, TEntity> repo, TEntity entity)
            where TKey : struct
            where TEntity : BgcEntity<TKey>
        {
            Shield.ArgumentNotNull(entity).ThrowOnError();

            repo.Delete(entity.Id);
        }
    }
}
