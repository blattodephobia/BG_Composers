using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Services
{
	internal abstract class ServiceBase
	{
		private static Dictionary<Type, Func<object, IEnumerable<IDbConnect>>> DbConnectMemberAccessors = new Dictionary<Type, Func<object, IEnumerable<IDbConnect>>>();

		/// <summary>
		/// Dynamically generates a method that will return the values of all properties implementing a specific type in an IEnumerable of that type.
		/// For example, when invoked with <paramref name="propertyDeclaringType"/> = typeof(<see cref="System.Tuple&lt;double, double&gt;"/>) and
		/// <typeparamref name="TPropertyType"/> is <see cref="System.Double"/> the generated method's return value will the values of the Item1 and
		/// Item2 properties. If, however, <paramref name="propertyDeclaringType"/> = typeof(<see cref="System.Tuple&lt;double, double&gt;"/>) and 
		/// <typeparamref name="TPropertyType"/> is <see cref="System.Int32" an empty collection will be returned./>
		/// </summary>
		/// <typeparam name="TPropertyType"></typeparam>
		/// <param name="propertyDeclaringType">The delcaring type. Must be a reference type.</param>
		/// <returns></returns>		
		private static Func<object, IEnumerable<TPropertyType>> GetPropertyValuesOfTypeAccessor<TPropertyType>(Type propertyDeclaringType)
		{
			IEnumerable<PropertyInfo> matchingProperties = propertyDeclaringType
														   .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
														   .Where(property => typeof(TPropertyType).IsAssignableFrom(property.PropertyType));
			ParameterExpression parameter = Expression.Parameter(typeof(object));
			var lambda = Expression.Lambda<Func<object, IEnumerable<TPropertyType>>>
			(
				parameters: parameter,
				body: Expression.NewArrayInit
				(
					type: typeof(TPropertyType),
					initializers: matchingProperties.Select(property => Expression.Property(Expression.Convert(parameter, propertyDeclaringType), property)) // (<parameter> as <propertyDeclaringType>).property
				)
			);
			return lambda.Compile();
		}

		private IEnumerable<IDbConnect> dbConnectedObjects;
		protected IEnumerable<IDbConnect> DbConnectedObjects
		{
			get
			{
				if (this.dbConnectedObjects == null)
				{
					this.dbConnectedObjects = DbConnectMemberAccessors[this.GetType()].Invoke(this);
				}

				return this.dbConnectedObjects ?? Enumerable.Empty<IDbConnect>();
			}
		}

		private IUnitOfWork commonUnitOfWork;
		protected IUnitOfWork CommonUnitOfWork
		{
			get
			{
				try
				{
					if (this.commonUnitOfWork == null)
					{
						this.commonUnitOfWork = this.DbConnectedObjects.Distinct().Single().UnitOfWork;
					}
					return this.commonUnitOfWork;
				}
				catch (InvalidOperationException)
				{
					if (!this.DbConnectedObjects.Any()) throw new InvalidOperationException("There are no objects connected to a database.");
					else throw new InvalidOperationException(string.Format("Database connectable objects are connected to different {0} instances", typeof(IUnitOfWork).Name));
				}
			}
		}

		protected ServiceBase()
		{
			Type currentType = this.GetType();
			if (!DbConnectMemberAccessors.ContainsKey(currentType))
			{
				var lambda = GetPropertyValuesOfTypeAccessor<IDbConnect>(currentType);
				//DbConnectMemberAccessors.Add(currentType, lambda);
			}
		}
	}
}
