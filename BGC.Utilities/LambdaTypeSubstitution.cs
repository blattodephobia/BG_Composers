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
    public class LambdaTypeSubstitution<TSourceParam, TTargetParam>
        where TTargetParam : TSourceParam
    {
        private class Visitor : ExpressionVisitor
        {
            private static readonly BindingFlags MemberLookupFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            private Type _sourceType;
            private Type _destinationType;
            private Dictionary<string, ParameterExpression> _paramsDictionary = new Dictionary<string, ParameterExpression>();
            
            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node.Type == _sourceType)
                {
                    if (!_paramsDictionary.ContainsKey(node.Name))
                    {
                        _paramsDictionary.Add(node.Name, Expression.Parameter(_destinationType, node.Name));
                    }

                    return _paramsDictionary[node.Name];
                }

                return node;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                Expression result = null;
                if (node.Member.DeclaringType == _sourceType)
                {
                    MemberInfo targetMember = _destinationType.GetMember(node.Member.Name, MemberLookupFlags)[0];
                    result = Expression.MakeMemberAccess(Visit(node.Expression), targetMember);
                }
                else
                {
                    result = base.VisitMember(node);
                }

                return result;
            }

            public Visitor(Type sourceType, Type destinationType, params ParameterExpression[] parameters)
            {
            _sourceType = sourceType;
            _destinationType = destinationType;
            _paramsDictionary = parameters.ToDictionary(pe => pe.Name);

            }
        }
        
        private IEnumerable<ParameterExpression> ConvertParameters<TResult>(Expression<Func<TSourceParam, TResult>> lambda)
        {
            var result = from param in lambda.Parameters
                         let convertedParam = param.Type == typeof(TSourceParam)
                             ? Expression.Parameter(typeof(TTargetParam), param.Name)
                             : param
                         select convertedParam;

            return result;
        }

        public Expression<Func<TTargetParam, TResult>> ChangeLambdaType<TResult>(Expression<Func<TSourceParam, TResult>> lambda)
        {
            Shield.ArgumentNotNull(lambda).ThrowOnError();

            ParameterExpression[] targetParams = ConvertParameters(lambda).ToArray();
            ExpressionVisitor visitor = new Visitor(typeof(TSourceParam), typeof(TTargetParam), targetParams);

            return Expression.Lambda<Func<TTargetParam, TResult>>(visitor.Visit(lambda.Body), targetParams.ToArray());
        }
    }
}
