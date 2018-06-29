using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data
{
	public interface IDbConnect : IDisposable
	{
		IUnitOfWork UnitOfWork { get; }
	}
}
