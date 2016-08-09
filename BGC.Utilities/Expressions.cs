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
        private static readonly Dictionary<Type, IDynamicMemberGetter> CachedGetters = new Dictionary<Type, IDynamicMemberGetter>();

		private static Expression RemoveConvert(Expression expression)
		{
			// Expressions using constants use their values rather than names after compilation. Therefore,
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

        private static Expression RemoveMemberAccess(Expression expression)
        {
            Expression result = expression;
            while (result.NodeType == ExpressionType.MemberAccess)
            {
                result = (result as MemberExpression).Expression;
            }

            return result;
        }

        private static object ExtractValue(Expression argumentExpression)
        {
            Shield.AssertOperation(
                argumentExpression,
                (argumentExpression is ConstantExpression) || (argumentExpression is MemberExpression),
                "The expression's value cannot be determined");

            ConstantExpression argument = RemoveMemberAccess(argumentExpression) as ConstantExpression;
            object result = null;
            if (argumentExpression is MemberExpression)
            {
                MemberExpression targetMember = argumentExpression as MemberExpression;
                if (!CachedGetters.ContainsKey(argument.Type))
                {
                    CachedGetters.Add(argument.Type, Activator.CreateInstance(typeof(LambdaCaptureFieldAccessor<>).MakeGenericType(argument.Type)) as IDynamicMemberGetter);
                }
                result = CachedGetters[argument.Type].GetMemberValue(argument.Value, targetMember.Member.Name);
            }
            else
            {
                result = (argumentExpression as ConstantExpression).Value;
            }

            return result;
        }

        private static string GetQueryStringInternal(MethodCallExpression call, bool includeNullValueParams)
        {
            ParameterInfo[] parameters = call.Method.GetParameters();
            List<string> paramValuePairs = new List<string>(call.Arguments.Count);
            for (int i = 0; i < call.Arguments.Count; i++)
            {
                Shield.AssertOperation(
                        call.Arguments[i] as MethodCallExpression,
                        argumentAsMethodCall => argumentAsMethodCall == null,
                        x => $"Cannot determine the return value of {x.Method.Name}; nested method call expressions are not supported.")
                    .ThrowOnError();
                
                string value = ExtractValue(call.Arguments[i])?.ToString() ?? string.Empty;
                if (value != string.Empty || includeNullValueParams) // if ExtractValue actually returns an empty string, it's still as good as a null value
                {
                    string paramAssignment = $"{parameters[i].Name}={value}";
                    paramValuePairs.Add(paramAssignment);
                }
            }
            return paramValuePairs.ToStringAggregate("&");
        }

        /// <summary>
        /// Returns the name of a field or property from a given expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
		public static string NameOf<T>(Expression<Func<T>> memberExpression)
		{
			MemberExpression convertRemoved = RemoveConvert(memberExpression.Body) as MemberExpression;
			string memberName = convertRemoved.Member.Name;
			return memberName;
		}

        /// <summary>
        /// Returns the name of a field or property from a given expression.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
		public static string NameOf<TObject>(Expression<Func<TObject, object>> memberExpression)
		{
			MemberExpression convertRemoved = RemoveConvert(memberExpression.Body) as MemberExpression;
			string memberName = convertRemoved.Member.Name;
			return memberName;
		}

        /// <summary>
        /// Generates a query string with the parameters and their values from the expressed invokation of a method (without actually invoking it).
        /// </summary>
        /// <param name="methodCallExpression">A method call expression, such as () => Method("value1", "value2")</param>
        /// <param name="includeNullValueParams">Specifies whether parameters whose values are null or an empty string should be included in the returned string in the form of '&amp;param='.</param>
        /// <exception cref="InvalidOperationException">One or more of the method's paramters are the return values of another method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="methodCallExpression"/> is null.</exception>
        /// <returns>A query string in the form of "param1=value1&amp;param2=value2".</returns>
        public static string GetQueryString(Expression<Action> methodCallExpression, bool includeNullValueParams = false)
        {
            Shield.ArgumentNotNull(methodCallExpression, nameof(methodCallExpression)).ThrowOnError();

            MethodCallExpression call = (methodCallExpression.Body as MethodCallExpression).ValueNotNull();
            return GetQueryStringInternal(call, includeNullValueParams);
        }

        /// <summary>
        /// Generates a query string with the parameters and their values from the expressed invokation of a method (without actually invoking it).
        /// This method can get query strings without needing an object instance of the declaring type.
        /// </summary>
        /// <param name="methodCallExpression">A method call expression, such as (x) => x.Method("value1", "value2")</param>
        /// <param name="includeNullValueParams">Specifies whether parameters whose values are null or an empty string should be included in the returned string in the form of '&amp;param='.</param>
        /// <exception cref="InvalidOperationException">One or more of the method's paramters are the return values of another method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="methodCallExpression"/> is null.</exception>
        /// <returns>A query string in the form of "param1=value1&amp;param2=value2".</returns>
        public static string GetQueryString<T>(Expression<Action<T>> methodCallExpression, bool includeNullValueParams = false)
        {
            Shield.ArgumentNotNull(methodCallExpression, nameof(methodCallExpression)).ThrowOnError();

            MethodCallExpression call = (methodCallExpression.Body as MethodCallExpression).AssertOperation(c => c != null, $"Expression is not supported.");
            return GetQueryStringInternal(call, includeNullValueParams);
        }

        public static Uri GetUri(Expression<Action> methodCallExpression, bool includeNullValueParams = false)
        {
            Shield.ArgumentNotNull(methodCallExpression, nameof(methodCallExpression));

            UriBuilder result = new UriBuilder()
            {
                Path = (methodCallExpression.Body as MethodCallExpression).Method.Name,
                Query = GetQueryStringInternal(methodCallExpression.Body as MethodCallExpression, includeNullValueParams)
            };

            return result.Uri;
        }
        
        /// <summary>
        /// Dynamically generates a method that will select the values of all properties of a given type <typeparamref name="TPropertyType"/> in
        /// an <see cref="IEnumerable{TPropertyType}"/>.
        /// For example, when invoked with <paramref name="propertyDeclaringType"/> = typeof(<see cref="System.Tuple&lt;double, double&gt;"/>)
        /// and <typeparamref name="TPropertyType"/> is <see cref="double"/> the generated method will return an <see cref="IEnumerable{double}"/>
        /// with the values of the Item1 and Item2 properties.
        /// If, however, <paramref name="propertyDeclaringType"/> = typeof(<see cref="System.Tuple{double, double}"/>)
        /// and <typeparamref name="TPropertyType"/> is <see cref="int" an empty collection will be returned./>
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="propertyDeclaringType">The delcaring type. Must be a reference type.</param>
        /// <returns></returns>		
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
    }
}
