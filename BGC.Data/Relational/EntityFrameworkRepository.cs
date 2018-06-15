using BGC.Core;
using BGC.Data.Relational.Mappings;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational
{
    internal class EntityFrameworkRepository<TKey, TEntity, TRelationalDto> : INonQueryableRepository<TKey, TEntity>
        where TKey : struct
        where TEntity : BgcEntity<TKey>
        where TRelationalDto : RelationdalDtoBase, new()
    {
        private readonly IDbSet<TRelationalDto> _dbSet;
        private readonly RelationalMapper<TEntity, TRelationalDto> _mapper;

        public EntityFrameworkRepository(RelationalMapper<TEntity, TRelationalDto> mapper, IDbSet<TRelationalDto> dbSet)
        {
            Shield.ArgumentNotNull(mapper).ThrowOnError();
            Shield.ArgumentNotNull(dbSet).ThrowOnError();

            _mapper = mapper;
            _dbSet = dbSet;
        }

        public virtual void AddOrUpdate(TEntity entity)
        {
            Shield.ArgumentNotNull(entity).ThrowOnError();

            TRelationalDto dto = _dbSet.Find(entity.Id);
            if (dto == null)
            {
                dto = new TRelationalDto();
                _mapper.CopyData(entity, dto);
                _dbSet.Add(dto);
            }
            else
            {
                _mapper.CopyData(entity, dto);
            }
        }

        public virtual void Delete(params TKey[] keys)
        {
            Shield.ArgumentNotNull(keys).ThrowOnError();

            var deleteObjects = from key in keys
                                let dto = _dbSet.Find(key)
                                where dto != null
                                select dto;

            if (_dbSet is DbSet<TRelationalDto>)
            {
                (_dbSet as DbSet<TRelationalDto>).RemoveRange(deleteObjects);
            }
            else
            {
                foreach (TRelationalDto dto in deleteObjects)
                {
                    _dbSet.Remove(dto);
                }
            }
        }

        public virtual TEntity Find(TKey key)
        {
            TRelationalDto dto = _dbSet.Find(key);
            if (dto != null)
            {
                return _mapper.ToEntity(dto);
            }
            else
            {
                return null;
            }
        }
    }
}
