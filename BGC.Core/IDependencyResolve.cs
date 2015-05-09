using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	public interface IDependencyResolve<TInjectorObject>
	{
		void RegisterUnitOfWork(TInjectorObject helper);
		void RegisterGenericRepository(TInjectorObject helper);
	}
}
