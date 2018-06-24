using BGC.Core;
using BGC.Data.Relational.Mappings;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Repositories
{
    internal abstract class EntityFrameworkRepository<TKey, TEntity, TRelationalDto> : INonQueryableRepository<TKey, TEntity>
        where TKey : struct
        where TEntity : BgcEntity<TKey>
        where TRelationalDto : RelationdalDtoBase, new()
    {
        private readonly DbContext _dbContext;
        private readonly DomainBuilderBase<TRelationalDto, TEntity> _builder;
        private readonly DomainBreakdownBase<TEntity> _breakdown;

        protected DbContext DbContext => _dbContext;
        protected DomainBuilderBase<TRelationalDto, TEntity> Builder => _builder;

        public EntityFrameworkRepository(DomainBuilderBase<TRelationalDto, TEntity> builder, DomainBreakdownBase<TEntity> breakdown, DbContext context)
        {
            Shield.ArgumentNotNull(builder).ThrowOnError();
            Shield.ArgumentNotNull(breakdown).ThrowOnError();
            Shield.ArgumentNotNull(context).ThrowOnError();

            _builder = builder;
            _breakdown = breakdown;
            _dbContext = context;
        }

        protected abstract void AddOrUpdateInternal(TEntity entity);

        public void AddOrUpdate(TEntity entity)
        {
            Shield.ArgumentNotNull(entity).ThrowOnError();

            AddOrUpdateInternal(entity);
        }

        public virtual void Delete(params TKey[] keys)
        {
            Shield.ArgumentNotNull(keys).ThrowOnError();

            IDbSet<TRelationalDto> dbSet = _dbContext.Set<TRelationalDto>();
            var deleteObjects = from key in keys
                                let dto = dbSet.Find(key)
                                where dto != null
                                select dto;

            if (dbSet is DbSet<TRelationalDto>)
            {
                (dbSet as DbSet<TRelationalDto>).RemoveRange(deleteObjects);
            }
            else
            {
                foreach (TRelationalDto dto in deleteObjects)
                {
                    dbSet.Remove(dto);
                }
            }
        }

        public abstract TEntity Find(TKey key);
    }
}
