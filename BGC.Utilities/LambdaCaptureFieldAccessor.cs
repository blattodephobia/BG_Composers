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
        private static Expression PropertyOrField(Expression expression, MemberInfo member)
        {
            if (member is PropertyInfo)
            {
                PropertyInfo property = member as PropertyInfo;
                return Expression.Property((property.GetMethod ?? property.SetMethod).IsStatic ? null : expression, property);
            }
            else if (member is FieldInfo)
            {
                FieldInfo field = member as FieldInfo;
                return Expression.Field(field.IsStatic ? null : expression, field);
            }
            else
            {
                throw new InvalidOperationException($"{member} is not a property or field.");
            }
        }

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
                            ? Expression.Convert(PropertyOrField(parameter, member), typeof(object)) as Expression
                            : PropertyOrField(parameter, member) as Expression;
                        return Expression.Lambda<Func<T, object>>(body, parameter).Compile();
                    });
        }

        public object GetMemberValue(T instance, string memberName)
        {
            Shield.IsNotNullOrEmpty(memberName).ThrowOnError();
            Shield.AssertOperation(memberName, accessors.ContainsKey(memberName), $"{memberName} is not a valid member of {typeof(T).FullName}").ThrowOnError();

            try
            {
                return accessors[memberName].Invoke(instance);
            }
            catch (NullReferenceException)
            {
                throw new ArgumentNullException($"The {memberName} field or property is not static and requires an instance of {typeof(T).FullName}.");
            }
        }

        public object GetMemberValue(object obj, string memberName)
        {
            Shield.Assert(obj, obj == null || obj.GetType() == typeof(T), (x) => new InvalidOperationException($"Unsupported type; this {nameof(IDynamicMemberGetter)} supports only type {typeof(T).FullName}")).ThrowOnError();
            return GetMemberValue((T)obj, memberName);
        }
    }
}
