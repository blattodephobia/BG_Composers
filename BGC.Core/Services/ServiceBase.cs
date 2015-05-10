using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Services
{
	internal abstract class ServiceBase
	{
		protected virtual IUnitOfWork UnitOfWork { get; set; }

		protected ServiceBase(IUnitOfWork unitofWork)
		{
			this.UnitOfWork = unitofWork;
		}
	}
}
