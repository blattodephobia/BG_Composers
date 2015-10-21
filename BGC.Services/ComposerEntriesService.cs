using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Services
{
	internal class ComposerEntriesService : ServiceBase, IComposerEntriesService
	{
		public IRepository<Composer> Composers { get; private set; }

		public ComposerEntriesService(IRepository<Composer> composers)
		{
			this.Composers = composers;
		}

		public IQueryable<Composer> GetAllEntries()
		{
			return this.Composers.All();
		}
	}
}
