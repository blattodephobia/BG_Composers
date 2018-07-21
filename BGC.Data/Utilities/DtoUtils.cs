using BGC.Core.Exceptions;
using BGC.Data;
using BGC.Data.Relational;
using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    internal static class DtoUtils
    {
        private static readonly Dictionary<Type, object> CachedKeyAccessors = new Dictionary<Type, object>();

        public static object[] GetKeys<TRelationalDto>(TRelationalDto dto) where TRelationalDto : RelationdalDtoBase
        {
            Shield.ArgumentNotNull(dto).ThrowOnError();

            Type dtoType = typeof(TRelationalDto);
            Func<TRelationalDto, object[]> keyAccessor = null;
            if (!CachedKeyAccessors.ContainsKey(typeof(TRelationalDto)))
            {
                keyAccessor = Expressions.GetPropertiesAccessor<TRelationalDto>(GetKeyProperties<TRelationalDto>());
                CachedKeyAccessors.Add(dtoType, keyAccessor);
            }

            keyAccessor = keyAccessor ?? CachedKeyAccessors[typeof(TRelationalDto)] as Func<TRelationalDto, object[]>;
            return keyAccessor.Invoke(dto);
        }

        public static PropertyInfo[] GetKeyProperties<TDto>()
        {
            Type dtoType = typeof(TDto);
            
            PropertyInfo[] keyDecoratedProperties = typeof(TDto).GetProperties().Where(p => p.GetCustomAttribute<KeyAttribute>() != null).ToArray();
            PropertyInfo idProperty = dtoType.GetProperty("Id");
            bool hasIdentity = keyDecoratedProperties?.Length > 0 || idProperty != null;
            Shield.Assert(dtoType, hasIdentity, t => new MissingMemberException($"Type {typeof(TDto).FullName} has no property(ies) which can identify the DTO uniquely.")).ThrowOnError();
            
            PropertyInfo[] result = keyDecoratedProperties?.Length > 0
                ? keyDecoratedProperties
                : new[] { idProperty };
            return result;
        }

        public static PropertyInfo GetIdentityProperty<TDto>()
        {
            Type dtoType = typeof(TDto);
            string domainIdPropertyName = dtoType.GetCustomAttribute<IdentityAttribute>()?.IdentityPropertyName;
            PropertyInfo result = !string.IsNullOrWhiteSpace(domainIdPropertyName)
                ? dtoType.GetProperty(domainIdPropertyName)
                : null;

            Shield.Assert(dtoType, result != null, t => new MissingMemberException($"The type {dtoType} is not decorated with {typeof(IdentityAttribute)} or the attribute's value doesn't refer to a valid property's name (note: {typeof(IdentityAttribute)} is not inheritable).")).ThrowOnError();

            return result;
        }
    }
}
