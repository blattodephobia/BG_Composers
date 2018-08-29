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
                var param = node.Expression as ParameterExpression;
                if (param?.Type == _sourceType)
                {
                    MemberInfo targetMember = FindTargetMember(node);
                    result = Expression.MakeMemberAccess(Visit(node.Expression), targetMember);
                }
                else
                {
                    result = base.VisitMember(node);
                }

                return result;
            }

            private MemberInfo FindTargetMember(MemberExpression node)
            {
                string memberName = node.Member.Name;
                if (_sourceType.IsInterface)
                {
                    InterfaceMapping iMap = _destinationType.GetInterfaceMap(_sourceType);
                    PropertyInfo property = node.Member as PropertyInfo;
                    if (property != null)
                    {
                        int targetIndex = iMap.InterfaceMethods.IndexOf(m => m.Name == property.GetMethod.Name);
                        MemberInfo target = _destinationType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetMethod == iMap.TargetMethods[targetIndex]).First();
                        return target;
                    }
                    else
                    {
                        throw new NotSupportedException("At the moment, type substitution for lambdas is only supported for properties");
                    }
                }
                else
                {
                    return _destinationType.GetMember(memberName, MemberLookupFlags).FirstOrDefault();
                }
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
