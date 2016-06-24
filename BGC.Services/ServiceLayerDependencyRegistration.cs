using BGC.Core;
using BGC.Core.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services
{
	public class ServiceLayerDependencyRegistration : IDependencyRegistration<IUnityContainer>
	{
        public const string DefaultDataStorageDirectoryKey = "StorageDir";

		public void RegisterTypes(IUnityContainer helper)
		{
            helper.RegisterType<IComposerDataService, ComposerDataService>();
            helper.RegisterType<ISettingsService, SettingsService>(new InjectionFactory(c => new SettingsService(c.Resolve<IRepository<Setting>>())));
            helper.RegisterType<IDataStorageService<string>, FileSystemTextStorageService>();
		}
	}
}
