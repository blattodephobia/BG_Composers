using BGC.Core;
using BGC.Core.Services;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services
{
	internal class ComposerEntriesService : DbServiceBase, IComposerDataService
	{
		public IRepository<Composer> Composers { get; private set; }

		public ComposerEntriesService(IRepository<Composer> composers)
		{
            Shield.ArgumentNotNull(composers, nameof(composers)).ThrowOnError();
			this.Composers = composers;
		}

		public IQueryable<Composer> GetAllComposers()
		{
			return this.Composers.All();
		}

        public void Add(Composer composer)
        {
            Shield.ArgumentNotNull(composer, nameof(composer)).ThrowOnError();
            this.Composers.Insert(composer);
            this.Composers.UnitOfWork.SaveChanges();
        }
    }
}
