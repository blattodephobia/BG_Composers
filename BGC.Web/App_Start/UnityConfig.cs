using System;
using System.Linq;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using BGC.Core;
using BGC.Data;
using BGC.Web.Areas.Administration.Controllers;
using BGC.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web;
using System.Reflection;
using BGC.Web.Views;
using BGC.Core.Services;
using BGC.Services;
using System.Xml;
using System.IO;
using System.Configuration;
using System.Web.Hosting;

namespace BGC.Web.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        private static IDependencyRegistration<IUnityContainer> DependencyRegistrationProvider = new DataLayerDependencyRegistration();

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            new DataLayerDependencyRegistration().RegisterTypes(container);
            new ServiceLayerDependencyRegistration().RegisterTypes(container);

            string storageDir = ConfigurationManager.AppSettings[ServiceLayerDependencyRegistration.DefaultDataStorageDirectoryKey];
            container.RegisterInstance(
                ServiceLayerDependencyRegistration.DefaultDataStorageDirectoryKey,
                new DirectoryInfo(HostingEnvironment.MapPath(storageDir)));

            // Inject UserManager<BgcUser, long> into all controllers inheriting from AdministrationControllerBase
            container
                .RegisterType<AdministrationControllerBase>()
                .RegisterTypes(
                    types: AllClasses
                           .FromAssemblies(Assembly.GetAssembly(typeof(AdministrationControllerBase)))
                           .Where(t => typeof(AdministrationControllerBase).IsAssignableFrom(t)),
                    getInjectionMembers: (t) => new InjectionMember[]
                    {
                        new InjectionProperty(
                            Expressions.NameOf<AdministrationControllerBase>(obj => obj.UserManager),
                            container.Resolve<UserManager<BgcUser, long>>())
                    });

            container.RegisterType<SignInManager<BgcUser, long>>(new InjectionFactory(c =>
            {
                return new SignInManager<BgcUser, long>(c.Resolve<UserManager<BgcUser, long>>(), HttpContext.Current.GetOwinContext().Authentication);
            }));

            container.RegisterType<ILocalizationService, LocalizationService>
            (
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(c =>
                {
                    string xmlPath = HttpContext.Current.Request.PhysicalApplicationPath + @"Localization\Localization.xml";
                    using (Stream xmlStream = File.OpenRead(xmlPath))
                    {
                        XmlDocument localizationXml = new XmlDocument();
                        localizationXml.Load(xmlStream);
                        return new LocalizationService(localizationXml);
                    }
                })
            );
        }
    }
}
