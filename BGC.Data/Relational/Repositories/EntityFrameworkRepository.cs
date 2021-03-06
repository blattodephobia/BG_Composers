﻿using BGC.Core;
using BGC.Core.Exceptions;
using BGC.Data.Relational.Mappings;
using BGC.Utilities;
using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Repositories
{
    internal class EntityFrameworkRepository<TKey, TEntity, TRelationalDto> : INonQueryableRepository<TKey, TEntity>
        where TKey : struct
        where TEntity : BgcEntity<TKey>
        where TRelationalDto : RelationdalDtoBase
    {
        private readonly DbContext _dbContext;
        private readonly DomainTypeMapperBase<TEntity, TRelationalDto> _typeMapper;

        private TRelationalDto FindFromContext(TKey key)
        {
            return DbContext.Set<TRelationalDto>().FirstOrDefault(GetFindPredicate(key));
        }
        
        protected DbContext DbContext => _dbContext;
        protected DomainTypeMapperBase<TEntity, TRelationalDto> TypeMapper => _typeMapper;

        private PropertyInfo _identityProperty;
        protected virtual PropertyInfo IdentityProperty => _identityProperty ?? (_identityProperty = DtoUtils.GetIdentityProperty<TRelationalDto>());

        protected Expression<Func<TRelationalDto, bool>> GetFindPredicate(TKey key)
        {
            ParameterExpression dto = Expression.Parameter(typeof(TRelationalDto), nameof(dto));
            MemberExpression idProperty = Expression.Property(dto, IdentityProperty);
            var result = Expression.Lambda<Func<TRelationalDto, bool>>(Expression.Equal(idProperty, Expression.Constant(key)), dto);

            return result;
        }

        public EntityFrameworkRepository(DomainTypeMapperBase<TEntity, TRelationalDto> typeMapper, DbContext context)
        {
            Shield.ArgumentNotNull(typeMapper).ThrowOnError();
            Shield.ArgumentNotNull(context).ThrowOnError();

            _typeMapper = typeMapper;
            _dbContext = context;
        }

        protected virtual void AddOrUpdateInternal(TEntity entity)
        {
            TRelationalDto dto = _typeMapper.BuildDto(entity);
            DbSet<TRelationalDto> set = DbContext.Set<TRelationalDto>();

            if (FindFromContext(entity.Id) == null)
            {
                set.Add(dto);
            }
        }

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

        public virtual TEntity Find(TKey key)
        {
            TRelationalDto dto = FindFromContext(key);
            if (dto != null)
            {
                return TypeMapper.Build(dto);
            }

            return null;
        }

        public void SaveChanges() => _dbContext.SaveChanges();
    }
}
