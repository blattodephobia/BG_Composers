﻿using BGC.Core;
using BGC.Data.Relational;
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

		public MySqlRepository(ComposersDbContext context)
		{
			this.UnitOfWork = context;
			this.DataSet = context.Set<T>();
		}

		public void Dispose()
		{
			this.UnitOfWork?.Dispose();
		}

        public void Insert(T entity)
        {
            this.DataSet.Add(entity);
        }

        public void Delete(T entity)
        {
            UnitOfWork.SetState(entity, EntityState.Deleted);
        }
    }
}
