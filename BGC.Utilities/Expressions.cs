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
        private static readonly Dictionary<Type, WeakReference<IDynamicMemberGetter>> CachedGetters = new Dictionary<Type, WeakReference<IDynamicMemberGetter>>();

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

            MemberExpression memberAccess = argumentExpression as MemberExpression;
            return ResolveMemberAccessRecursively(memberAccess, memberAccess?.Expression ?? argumentExpression);
        }

        /* Expressions such as () => obj.Property1.Property2; are transformed in the following Expression tree: 
         * 
         *      MemberExpression:
         *      +---Member:     Property2
         *      +---Expression: MemberExpression:
         *                      +---Member:     Property1
         *                      +---Expression: MemberExpression:
         *                                      +---Member: obj
         *                                      +----Expression: ConstantExpression:
         *                                                       +--- Value: <>AnonymousType
         *                                                       
         * Therefore, we recursively have to get the member of the value in the Expression property.
         * The very last Expression node is of type MemberExpression with a concrete, immediately
         * available value. That's where the bottom of the recursion is. */
        private static object ResolveMemberAccessRecursively(MemberExpression targetMember, Expression subExpression)
        {
            object result = !(subExpression is ConstantExpression)
                ? GetValue(memberName: targetMember.Member.Name,
                           obj:        ResolveMemberAccessRecursively(targetMember.Expression as MemberExpression, (targetMember as MemberExpression).Expression))
                : GetValue(memberName: targetMember?.Member.Name, // return <ConstantExpression> if targetMember is null, otherwise <ConstantExpression>.targetMember;
                           obj: (subExpression as ConstantExpression).Value);

            return result;
        }

        private static object GetValue(object obj, string memberName)
        {
            if (string.IsNullOrEmpty(memberName))
            {
                return obj;
            }

            Type objType = obj.GetType();
            IDynamicMemberGetter valueGetter = null;
            if (!CachedGetters.ContainsKey(objType))
            {
                valueGetter = Activator.CreateInstance(typeof(LambdaCaptureFieldAccessor<>).MakeGenericType(objType)) as IDynamicMemberGetter;
                var reference = new WeakReference<IDynamicMemberGetter>(valueGetter);
                CachedGetters.Add(objType, reference);
            }
            else if (!CachedGetters[objType].TryGetTarget(out valueGetter))
            {
                valueGetter = Activator.CreateInstance(typeof(LambdaCaptureFieldAccessor<>).MakeGenericType(objType)) as IDynamicMemberGetter;
                CachedGetters[objType].SetTarget(valueGetter);
            }
            return valueGetter.GetMemberValue(obj, memberName);
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
        /// For example, when invoked with <typeparamref name="TDeclaringType"/> is <see cref="System.Tuple&lt;double, double&gt;"/>
        /// and <typeparamref name="TPropertyType"/> is <see cref="double"/> the generated method will return an <see cref="IEnumerable{double}"/>
        /// with the values of the Item1 and Item2 properties.
        /// If, however, <typeparamref name="TDeclaringType"/> is <see cref="System.Tuple{double, double}"/>
        /// and <typeparamref name="TPropertyType"/> is <see cref="int"/> an empty collection will be returned.
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <typeparam name="TDeclaringType">The delcaring type.</param>
        /// <returns></returns>		
        public static Func<TDeclaringType, IEnumerable<TPropertyType>> GetPropertyValuesOfTypeAccessor<TDeclaringType, TPropertyType>()
        {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            IEnumerable<PropertyInfo> matchingProperties = from property in typeof(TDeclaringType).GetProperties(flags)
                                                           where typeof(TPropertyType).IsAssignableFrom(property.PropertyType)
                                                           select property;

            return GetPropertyValuesOfTypeAccessor<TDeclaringType, TPropertyType>(matchingProperties);
        }

        /// <summary>
        /// Dynamically generates a method that will select the values of all specified properties of type <typeparamref name="TPropertyType"/> in
        /// an <see cref="IEnumerable{TPropertyType}"/>.
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <typeparam name="TDeclaringType">The delcaring type.</param>
        /// <returns></returns>		
        public static Func<TDeclaringType, IEnumerable<TPropertyType>> GetPropertyValuesOfTypeAccessor<TDeclaringType, TPropertyType>(IEnumerable<PropertyInfo> properties)
        {
            Shield.ArgumentNotNull(properties).ThrowOnError();

            ParameterExpression parameter = Expression.Parameter(typeof(TDeclaringType));
            var lambda = Expression.Lambda<Func<TDeclaringType, IEnumerable<TPropertyType>>>
            (
                parameters: parameter,
                body: Expression.NewArrayInit
                (
                    type: typeof(TPropertyType),
                    initializers: properties.Select(property =>
                    {
                        return Expression.Property(parameter, property); // ((<propertyDeclaringType>)<parameter>).property
                    })
                )
            );

            return lambda.Compile();
        }

        /// <summary>
        /// Dynamically generates a method that will select the values of all specified properties of type <typeparamref name="TPropertyType"/> in
        /// an <see cref="IEnumerable{TPropertyType}"/>.
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <returns></returns>		
        public static Func<object, IEnumerable<TPropertyType>> GetPropertyValuesOfTypeAccessor<TPropertyType>(Type declaringType, IEnumerable<PropertyInfo> properties)
        {
            Shield.ArgumentNotNull(properties).ThrowOnError();

            ParameterExpression parameter = Expression.Parameter(typeof(object));
            var lambda = Expression.Lambda<Func<object, IEnumerable<TPropertyType>>>
            (
                parameters: parameter,
                body: Expression.NewArrayInit
                (
                    type: typeof(TPropertyType),
                    initializers: properties.Select(property =>
                    {
                        return Expression.Property(Expression.Convert(parameter, declaringType), property); // ((<propertyDeclaringType>)<parameter>).property
                    })
                )
            );

            return lambda.Compile();
        }

        public static Func<object, IEnumerable<TPropertyType>> GetPropertyValuesOfTypeAccessor<TPropertyType>(Type propertyDeclaringType)
        {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            IEnumerable<PropertyInfo> matchingProperties = from property in propertyDeclaringType.GetProperties(flags)
                                                           where typeof(TPropertyType).IsAssignableFrom(property.PropertyType)
                                                           select property;

            ParameterExpression parameter = Expression.Parameter(typeof(object));
            var lambda = Expression.Lambda<Func<object, IEnumerable<TPropertyType>>>
            (
                parameters: parameter,
                body: Expression.NewArrayInit
                (
                    type: typeof(TPropertyType),
                    initializers: matchingProperties.Select(property =>
                    {
                        return Expression.Property(Expression.Convert(parameter, propertyDeclaringType), property); // ((<propertyDeclaringType>)<parameter>).property
                    })
                )
            );

            return lambda.Compile();
        }
    }
}
