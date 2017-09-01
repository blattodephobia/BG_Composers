using BGC.Core;
using BGC.Core.Services;
using CodeShield;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services
{
	public class ServiceLayerDependencyRegistration : IDependencyRegistration<IUnityContainer, LifetimeManager>
	{
        private static readonly Dictionary<Type, Action<IUnityContainer, LifetimeManager>> RegistrationDelegates = new Dictionary<Type, Action<IUnityContainer, LifetimeManager>>()
        {
            { typeof(IComposerDataService),     (c, lm) => c.RegisterType<IComposerDataService, ComposerDataService>() },
            { typeof(ISettingsService),         (c, lm) => c.RegisterType<ISettingsService, SettingsService>(new InjectionFactory(c2 => new SettingsService(c2.Resolve<IRepository<Setting>>()))) },
            { typeof(IArticleContentService),   (c, lm) => c.RegisterType<IArticleContentService, FileSystemArticleContentService>() },
            { typeof(IMediaService),            (c, lm) => c.RegisterType<IMediaService, FileSystemMediaService>() },
            { typeof(IGlossaryService),         (c, lm) => c.RegisterType<IGlossaryService, GlossaryService>() }
        };

        public const string DefaultDataStorageDirectoryKey = "DataStorageDir";

        public const string DefaultMediaStorageDirectoryKey = "MediaStorageDir";

        private readonly IUnityContainer container;

        public ServiceLayerDependencyRegistration()
        {
        }

        public ServiceLayerDependencyRegistration(IUnityContainer container)
        {
            this.container = container;
        }

        public void RegisterType(Type type, LifetimeManager scope = null)
        {
            Shield.AssertOperation(
                this.container,
                c => c != null,
                $"This overload of {nameof(RegisterType)} requires that the current instance of {nameof(ServiceLayerDependencyRegistration)} has been initialized with a valid instance of {nameof(IUnityContainer)}.").ThrowOnError();

            RegisterType(type, container.AssertOperation(this.container != null, nameof(this.container)).GetValueOrThrow(), scope);
        }

        public void RegisterType(Type type, IUnityContainer helper, LifetimeManager scope = null)
        {
            Shield.ArgumentNotNull(helper, nameof(helper)).ThrowOnError();
            Shield.ArgumentNotNull(type, nameof(type)).ThrowOnError();
            Shield.AssertOperation(type, t => RegistrationDelegates.ContainsKey(t), $"The type {type.FullName} is not supported by this assembly and cannot be registered.").ThrowOnError();

            RegistrationDelegates[type].Invoke(helper, scope);
        }
	}
}
