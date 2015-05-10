using BGC.Core;
using BGC.Core.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data
{
	public class UnityDependencyInjector : IDependencyRegistration<IUnityContainer>
	{
		public void RegisterUnitOfWork(IUnityContainer helper)
		{
			helper.RegisterType<IUnitOfWork, ComposersDbContext>(new InjectionConstructor());
		}

		public void RegisterServices(IUnityContainer helper)
		{
			helper.RegisterType<IComposerEntriesService, ComposerEntriesService>();
		}
	}
}
