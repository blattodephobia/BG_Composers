using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    internal class LambdaCaptureFieldAccessor<T> : IDynamicMemberGetter
    {
        private readonly Dictionary<string, Func<T, object>> accessors;

        public LambdaCaptureFieldAccessor()
        {
            // generate and compile methods that directly return a single field or property of an object;
            // e.g. x => x.field;
            this.accessors = typeof(T).GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(member => member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property)
                .ToDictionary(
                    member => member.Name,
                    member =>
                    {
                        ParameterExpression parameter = Expression.Parameter(member.DeclaringType);
                        Type memberType = member.MemberType == MemberTypes.Field ? (member as FieldInfo).FieldType : (member as PropertyInfo).PropertyType;
                        Expression body = memberType.IsValueType // if the type is a value type, we need to perform boxing
                            ? Expression.Convert(Expression.PropertyOrField(parameter, member.Name), typeof(object)) as Expression
                            : Expression.PropertyOrField(parameter, member.Name) as Expression;
                        return Expression.Lambda<Func<T, object>>(body, parameter).Compile();
                    });
        }

        public object GetMemberValue(T instance, string memberName)
        {
            Shield.ArgumentNotNull(instance).ThrowOnError();
            return accessors[memberName].Invoke(instance);
        }

        public object GetMemberValue(object obj, string memberName)
        {
            Shield.ArgumentNotNull(obj).ThrowOnError();
            Shield.Assert(obj, obj is T, (val) => new InvalidOperationException($"Unsupported type; this {nameof(IDynamicMemberGetter)} supports only type {typeof(T).FullName}")).ThrowOnError();
            return GetMemberValue((T)obj, memberName);
        }
    }
}
