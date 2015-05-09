using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data
{
	internal class MySqlRepository<T> : IRepository<T>
	{
		public IUnitOfWork UnitOfWork { get; private set; }

		public MySqlRepository(IUnitOfWork unitOfWork)
		{
			this.UnitOfWork = unitOfWork;
		}
	}
}