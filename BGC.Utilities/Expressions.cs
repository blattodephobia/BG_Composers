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
            Shield
                .Assert(argumentExpression, (argumentExpression is ConstantExpression) || (argumentExpression is MemberExpression), "The expression's value cannot be determined")
                .ThrowOnError();

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
        /// <param name="includeNullValueParams">Specifies whether parameters whose values are null should be included in the returned string in the form of '&param='.</param>
        /// <exception cref="InvalidOperationException">One or more of the method's paramters are the return values of another method.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="methodCallExpression"/> is null.</exception>
        /// <returns>A query string in the form of "param1=value1&param2=value2".</returns>
        public static string GetQueryString(Expression<Action> methodCallExpression, bool includeNullValueParams = false)
        {
            Shield.ArgumentNotNull(methodCallExpression, nameof(methodCallExpression)).ThrowOnError();

            MethodCallExpression call = (methodCallExpression.Body as MethodCallExpression).ValueNotNull().GetValueOrThrow();
            ParameterInfo[] parameters = call.Method.GetParameters();
            List<string> paramValuePairs = new List<string>(call.Arguments.Count);
            for (int i = 0; i < call.Arguments.Count; i++)
            {
                Shield
                    .Assert(
                        call.Arguments[i],
                        (expr) => !(call.Arguments[i] is MethodCallExpression),
                        (expr) => new InvalidOperationException($"Cannot determine the return value of {(expr as MethodCallExpression).Method.Name}; nested method call expressions are not supported."))
                    .ThrowOnError();
                string value = ExtractValue(call.Arguments[i])?.ToString();
                if (value != null || includeNullValueParams)
                {
                    string paramAssignment = $"{parameters[i].Name}={value}";
                    paramValuePairs.Add(paramAssignment);
                }
            }
            return paramValuePairs.ToStringAggregate("&");
        }
	}
}
