using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Services
{
	public interface IComposerEntriesService
	{
		IQueryable<Composer> GetAllEntries();
	}
}
