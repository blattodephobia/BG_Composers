using BGC.Core;
using BGC.Data;
using BGC.Utilities;
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
                    var lambda = Expressions.GetPropertyValuesOfTypeAccessor<IDbConnect>(currentType);
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
