using BGC.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data
{
	internal class MySqlRepository<T> : IRepository<T>
		where T : class
	{
		protected DbSet<T> DataSet { get; set; }

		public IUnitOfWork UnitOfWork { get; protected set; }

		public IQueryable<T> All()
		{
			return this.DataSet;
		}

		public MySqlRepository(IUnitOfWork unitOfWork, DbSet<T> set)
		{
			this.UnitOfWork = unitOfWork;
			this.DataSet = set;
		}
	}
}
