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
	public static class Expressions
	{
		private static Expression RemoveConvert(Expression expression)
		{
			// Expressions using constants use their values rather names after compilation. Therefore,
			// any information regarding the constant's name is stripped from the Expression object.
			Shield.Assert(expression, !(expression is ConstantExpression), x => new InvalidOperationException("Constant member access expressions are not supported.")).ThrowOnError();

			Expression result = expression;
			while (
				result.NodeType == ExpressionType.Convert ||
				result.NodeType == ExpressionType.ConvertChecked)
			{
				result = ((UnaryExpression)result).Operand;
			}

			return result;
        }

        /// <summary>
        /// Dynamically generates a method that will select the values of all properties of a given type <typeparamref name="TPropertyType"/> in
        /// an <see cref="IEnumerable{TPropertyType}"/>.
        /// For example, when <typeparamref name="TDeclaringType"/> is <see cref="System.Tuple{double, double}"/>
        /// and <typeparamref name="TPropertyType"/> is <see cref="double"/> the generated method will return an <see cref="IEnumerable{double}"/>
        /// with the values of the Item1 and Item2 properties.
        /// If, however, <paramref name="propertyDeclaringType"/> = typeof(<see cref="System.Tuple{double, double}"/>)
        /// and <typeparamref name="TPropertyType"/> is <see cref="int" an empty collection will be returned./>
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="propertyDeclaringType">The delcaring type. Must be a reference type.</param>
        /// <returns>A method that when called will return an <see cref="IEnumerable{TPropertyType}"/> with the values of all properties of type <typeparamref cref="TPropertyType"/>.</returns>	
        public static Func<TDeclaringType, IEnumerable<TPropertyType>> GetPropertyValuesOfTypeAccessor<TDeclaringType, TPropertyType>()
        {
            IEnumerable<PropertyInfo> matchingProperties =
                typeof(TDeclaringType)
                .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(property => typeof(TPropertyType).IsAssignableFrom(property.PropertyType));
            ParameterExpression parameter = Expression.Parameter(typeof(TDeclaringType));
            var lambda = Expression.Lambda<Func<TDeclaringType, IEnumerable<TPropertyType>>>
            (
                parameters: parameter,
                body: Expression.NewArrayInit
                (
                    type: typeof(TPropertyType),
                    initializers: matchingProperties.Select(property =>
                    {
                        return Expression.Property(parameter, property); // <parameter>.property
                    })
                )
            );
            return lambda.Compile();
        }

        /// <summary>
        /// Dynamically generates a method that will select the values of all properties of a given type <typeparamref name="TPropertyType"/> in
        /// an <see cref="IEnumerable{TPropertyType}"/>.
        /// For example, when invoked with <paramref name="propertyDeclaringType"/> = typeof(<see cref="System.Tuple&lt;double, double&gt;"/>)
        /// and <typeparamref name="TPropertyType"/> is <see cref="Double"/> the generated method will return an <see cref="IEnumerable{double}"/>
        /// with the values of the Item1 and Item2 properties.
        /// If, however, <paramref name="propertyDeclaringType"/> = typeof(<see cref="System.Tuple{double, double}"/>)
        /// and <typeparamref name="TPropertyType"/> is <see cref="Int32" an empty collection will be returned./>
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="propertyDeclaringType">The delcaring type. Must be a reference type.</param>
        /// <returns>A method that when called will return an <see cref="IEnumerable{TPropertyType}"/> with the values of all properties of type <typeparamref cref="TPropertyType"/> in the specified object.</returns>	
        public static Func<object, IEnumerable<TPropertyType>> GetPropertyValuesOfTypeAccessor<TPropertyType>(Type propertyDeclaringType)
        {
            IEnumerable<PropertyInfo> matchingProperties =
                propertyDeclaringType
                .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(property => typeof(TPropertyType).IsAssignableFrom(property.PropertyType));
            ParameterExpression parameter = Expression.Parameter(typeof(object));
            var lambda = Expression.Lambda<Func<object, IEnumerable<TPropertyType>>>
            (
                parameters: parameter,
                body: Expression.NewArrayInit
                (
                    type: typeof(TPropertyType),
                    initializers: matchingProperties.Select(property =>
                    {
                        Expression typeConvert = Expression.Convert(parameter, propertyDeclaringType);
                        return Expression.Property(typeConvert, property); // ((<propertyDeclaringType>)<parameter>).property
                    })
                )
            );
            return lambda.Compile();
        }

        public static string NameOf<T>(Expression<Func<T>> memberExpression)
		{
			MemberExpression convertRemoved = RemoveConvert(memberExpression.Body) as MemberExpression;
			string memberName = convertRemoved.Member.Name;
			return memberName;
		}

		public static string NameOf<TObject>(Expression<Func<TObject, object>> memberExpression)
		{
			MemberExpression convertRemoved = RemoveConvert(memberExpression.Body) as MemberExpression;
			string memberName = convertRemoved.Member.Name;
			return memberName;
		}
	}
}
