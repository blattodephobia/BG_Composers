using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	public interface IDependencyRegistration<TInjectorObject, TScope>
	{
		void RegisterType(Type type, TInjectorObject helper, TScope scope = default(TScope));
	}
}
