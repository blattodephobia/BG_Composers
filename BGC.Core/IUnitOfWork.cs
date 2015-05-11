using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    internal interface IUnitOfWork : IDisposable
    {
		void MarkUpdated<T>(T entity)
			where T : class;
    }
}
