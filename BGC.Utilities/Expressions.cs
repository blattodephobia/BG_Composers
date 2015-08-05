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
			DebugCheck.IsFalse(expression is ConstantExpression, () => new InvalidOperationException("Constant member access expressions are not supported."));

			Expression result = expression;
			while (
				result.NodeType == ExpressionType.Convert ||
				result.NodeType == ExpressionType.ConvertChecked)
			{
				result = ((UnaryExpression)result).Operand;
			}

			return result;
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
