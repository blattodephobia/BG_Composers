using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	internal interface IDbConnect : IDisposable
	{
		IUnitOfWork UnitOfWork { get; }
	}
}
