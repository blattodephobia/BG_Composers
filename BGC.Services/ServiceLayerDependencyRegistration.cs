using BGC.Core;
using BGC.Core.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services
{
	public class ServiceLayerDependencyRegistration : IDependencyRegistration<IUnityContainer>
	{
		public void RegisterTypes(IUnityContainer helper)
		{
            helper.RegisterType<IComposerEntriesService, ComposerEntriesService>();
		}
	}
}
