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
        private static Dictionary<Type, Func<object, IEnumerable<IDbPersist>>> DbPersistMemberAccessors = new Dictionary<Type, Func<object, IEnumerable<IDbPersist>>>();

        private readonly Type _currentType;

        private IUnitOfWork _commonUnitOfWork;
		protected IUnitOfWork CommonUnitOfWork
		{
			get
			{
				if (_commonUnitOfWork == null)
				{
					IEnumerable<IDbConnect> dbConnectedObjects = DbConnectMemberAccessors[this._currentType].Invoke(this) ?? Enumerable.Empty<IDbConnect>();
					IEnumerable<IUnitOfWork> unitOfWorkInstances = dbConnectedObjects.Select(obj => obj.UnitOfWork);
					if (!unitOfWorkInstances.Any()) throw new InvalidOperationException("There are no objects connected to a database.");
                    
					_commonUnitOfWork = unitOfWorkInstances.Aggregate((prev, curr) =>
					{
						if (object.ReferenceEquals(prev, curr)) return curr;
						else throw new InvalidOperationException(string.Format("Database connected objects use more than one {0} instance", typeof(IUnitOfWork).Name));
					});
				}
				return _commonUnitOfWork;
			}
		}

        protected IEnumerable<IDbConnect> GetDatbaseConnectedObjects()
        {
            return DbConnectMemberAccessors[_currentType].Invoke(this);
        }

        protected IEnumerable<IDbPersist> GetDatabaseConnectedObjects2()
        {
            return DbPersistMemberAccessors[_currentType].Invoke(this);
        }
        
        protected void SaveAll()
        {
            foreach (IDbConnect dbConnection in GetDatbaseConnectedObjects())
            {
                dbConnection.UnitOfWork.SaveChanges();
            }

            foreach (IDbPersist dbPeristObject in GetDatabaseConnectedObjects2())
            {
                dbPeristObject.SaveChanges();
            }
        }

		protected DbServiceBase()
		{
            lock (DbConnectMemberAccessors)
            {
                _currentType = GetType();
                if (!DbConnectMemberAccessors.ContainsKey(_currentType))
                {
                    var dbConnectLambda = Expressions.GetPropertyValuesOfTypeAccessor<IDbConnect>(_currentType);
                    DbConnectMemberAccessors.Add(_currentType, dbConnectLambda);

                    var dbPersistLambda = Expressions.GetPropertyValuesOfTypeAccessor<IDbPersist>(_currentType);
                    DbPersistMemberAccessors.Add(_currentType, dbPersistLambda);
                }
            }
		}

		public void Dispose()
		{
			CommonUnitOfWork.Dispose();
		}
	}
}
