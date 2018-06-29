using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data
{
    public interface IUnitOfWork : IDisposable
    {
		int SaveChanges();
		IRepository<T> GetRepository<T>()
			where T : class;

        void SetState<T>(T entity, EntityState state)
            where T : class;
    }
}
