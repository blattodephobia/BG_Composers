using BGC.Core;
using BGC.Core.Exceptions;
using BGC.Data.Relational.Mappings;
using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Repositories
{
    internal abstract class EntityFrameworkRepository<TKey, TEntity, TRelationalDto> : INonQueryableRepository<TKey, TEntity>
        where TKey : struct
        where TEntity : BgcEntity<TKey>
        where TRelationalDto : RelationdalDtoBase
    {
        private readonly DbContext _dbContext;
        private readonly DomainTypeMapperBase<TEntity, TRelationalDto> _typeMapper;
        private readonly RelationalPropertyMapper<TEntity, TRelationalDto> _propertyMapper;

        private PropertyInfo GetIdentityProperty()
        {
            string idPropertyName = typeof(TRelationalDto).GetCustomAttribute<IdentityAttribute>()?.IdentityPropertyName;
            if (idPropertyName == null)
            {
                PropertyInfo[] candidateProperties = typeof(TRelationalDto).GetProperties();
                int keyAttrCounts = 0, keyAttrFirstPos = -1, idNamePos = -1;
                for (int i = 0; i < candidateProperties.Length; i++)
                {
                    PropertyInfo curProp = candidateProperties[i];

                    if (curProp.GetCustomAttribute<KeyAttribute>() != null)
                    {
                        keyAttrCounts++;
                        keyAttrFirstPos = keyAttrFirstPos < 0 ? i : keyAttrFirstPos;
                    }

                    if (curProp.Name == "Id")
                    {
                        idNamePos = idNamePos < 0 ? i : idNamePos;
                    }
                }

                Shield.Assert(typeof(TRelationalDto), keyAttrCounts <= 1, (x) => new DuplicateKeyException($"Type {typeof(TRelationalDto).FullName} contains more than one property used to identify the DTO uniquely.")).ThrowOnError();
                Shield.Assert(typeof(TRelationalDto), keyAttrFirstPos >= 0 || idNamePos >= 0, (x) => new MissingMemberException($"Type {typeof(TRelationalDto).FullName} has no property which can identify the DTO uniquely.")).ThrowOnError();

                return candidateProperties[keyAttrFirstPos != -1 ? keyAttrFirstPos : idNamePos];
            }
            else
            {
                PropertyInfo idProperty = typeof(TRelationalDto).GetProperty(idPropertyName);
                Shield.Assert(typeof(TRelationalDto), idProperty != null, (x) => new TargetException($"Type {typeof(TRelationalDto).FullName} has no publicly accessible property named {idPropertyName}.")).ThrowOnError();

                return idProperty;
            }
        }

        protected DbContext DbContext => _dbContext;
        protected DomainTypeMapperBase<TEntity, TRelationalDto> TypeMapper => _typeMapper;

        private PropertyInfo _identityProperty;
        protected virtual PropertyInfo IdentityProperty => _identityProperty ?? (_identityProperty = GetIdentityProperty());

        protected Expression<Func<TRelationalDto, bool>> GetFindPredicate(TKey key)
        {
            ParameterExpression dto = Expression.Parameter(typeof(TRelationalDto), nameof(dto));
            MemberExpression idProperty = Expression.Property(dto, IdentityProperty);
            var result = Expression.Lambda<Func<TRelationalDto, bool>>(Expression.Equal(idProperty, Expression.Constant(key)), dto);

            return result;
        }

        public EntityFrameworkRepository(DomainTypeMapperBase<TEntity, TRelationalDto> typeMapper, RelationalPropertyMapper<TEntity, TRelationalDto> propertyMapper, DbContext context)
        {
            Shield.ArgumentNotNull(typeMapper).ThrowOnError();
            Shield.ArgumentNotNull(propertyMapper).ThrowOnError();
            Shield.ArgumentNotNull(context).ThrowOnError();

            _typeMapper = typeMapper;
            _propertyMapper = propertyMapper;
            _dbContext = context;
        }

        protected virtual void AddOrUpdateInternal(TEntity entity)
        {
            foreach (RelationdalDtoBase dto in _typeMapper.Breakdown(entity))
            {
                DbContext.Set(dto.GetType()).Add(dto);
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
            TRelationalDto dto = DbContext.Set<TRelationalDto>().FirstOrDefault(GetFindPredicate(key));
            if (dto != null)
            {
                return TypeMapper.Build(dto);
            }

            return null;
        }

        public void SaveChanges() => _dbContext.SaveChanges();
    }
}
