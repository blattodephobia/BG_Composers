using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services
{
	internal abstract class DbServiceBase : IDisposable
	{
		private static Dictionary<Type, Func<object, IEnumerable<IDbConnect>>> DbConnectMemberAccessors = new Dictionary<Type, Func<object, IEnumerable<IDbConnect>>>();

        /// <summary>
        /// Dynamically generates a method that will select the values of all properties of a given type <typeparamref name="TPropertyType"/> in
        /// an <see cref="IEnumerable{TPropertyType}"/>.
        /// For example, when invoked with <paramref name="propertyDeclaringType"/> = typeof(<see cref="System.Tuple&lt;double, double&gt;"/>)
        /// and <typeparamref name="TPropertyType"/> is <see cref="Double"/> the generated method return an <see cref="IEnumerable{double}"/>
        /// with the values of the Item1 and Item2 properties.
        /// If, however, <paramref name="propertyDeclaringType"/> = typeof(<see cref="System.Tuple{double, double}"/>)
        /// and <typeparamref name="TPropertyType"/> is <see cref="Int32" an empty collection will be returned./>
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="propertyDeclaringType">The delcaring type. Must be a reference type.</param>
        /// <returns></returns>		
        private static Func<object, IEnumerable<TPropertyType>> GetPropertyValuesOfTypeAccessor<TPropertyType>(Type propertyDeclaringType)
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

        private readonly Type currentType;

        private IUnitOfWork commonUnitOfWork;
		protected IUnitOfWork CommonUnitOfWork
		{
			get
			{
				if (this.commonUnitOfWork == null)
				{
					IEnumerable<IDbConnect> dbConnectedObjects = DbConnectMemberAccessors[this.currentType].Invoke(this) ?? Enumerable.Empty<IDbConnect>();
					IEnumerable<IUnitOfWork> unitOfWorkInstances = dbConnectedObjects.Select(obj => obj.UnitOfWork);
					if (!unitOfWorkInstances.Any()) throw new InvalidOperationException("There are no objects connected to a database.");
                    
					this.commonUnitOfWork = unitOfWorkInstances.Aggregate((prev, curr) =>
					{
						if (object.ReferenceEquals(prev, curr)) return curr;
						else throw new InvalidOperationException(string.Format("Database connected objects use more than one {0} instance", typeof(IUnitOfWork).Name));
					});
				}
				return this.commonUnitOfWork;
			}
		}

        protected IEnumerable<IDbConnect> GetDatbaseConnectedObjects()
        {
            return DbConnectMemberAccessors[this.currentType].Invoke(this);
        }
        
        protected void SaveAll()
        {
            foreach (IDbConnect dbConnection in GetDatbaseConnectedObjects())
            {
                dbConnection.UnitOfWork.SaveChanges();
            }
        }

		protected DbServiceBase()
		{
            lock (DbConnectMemberAccessors)
            {
                this.currentType = this.GetType();
                if (!DbConnectMemberAccessors.ContainsKey(currentType))
                {
                    var lambda = GetPropertyValuesOfTypeAccessor<IDbConnect>(currentType);
                    DbConnectMemberAccessors.Add(currentType, lambda);
                }
            }
		}

		public void Dispose()
		{
			this.CommonUnitOfWork.Dispose();
		}
	}
}
