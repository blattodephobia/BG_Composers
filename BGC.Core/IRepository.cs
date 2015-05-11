using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	internal interface IRepository<T> : IDbConnect
		where T : class
	{
		IQueryable<T> All();
	}
}
