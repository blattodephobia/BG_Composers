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
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

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
            var dataLayerDependencyRegistration = new DataLayerDependencyRegistration(container);
            dataLayerDependencyRegistration.RegisterType(typeof(IRepository<>));
            dataLayerDependencyRegistration.RegisterType(typeof(DbContext), new PerRequestLifetimeManager());
            dataLayerDependencyRegistration.RegisterType(typeof(IUnitOfWork), new PerRequestLifetimeManager());
            dataLayerDependencyRegistration.RegisterType(typeof(IUserStore<BgcUser, long>));
            dataLayerDependencyRegistration.RegisterType(typeof(IRoleStore<BgcRole, long>));

            var serviceLayerDependencyRegistration = new ServiceLayerDependencyRegistration(container);
            serviceLayerDependencyRegistration.RegisterType(typeof(IComposerDataService),   container);
            serviceLayerDependencyRegistration.RegisterType(typeof(ISettingsService),       container);
            serviceLayerDependencyRegistration.RegisterType(typeof(IArticleContentService), container);
            serviceLayerDependencyRegistration.RegisterType(typeof(IMediaService),          container);
            serviceLayerDependencyRegistration.RegisterType(typeof(IUserManagementService), container);
            
            string dataStorageDir = ConfigurationManager.AppSettings[ServiceLayerDependencyRegistration.DefaultDataStorageDirectoryKey];
            container.RegisterInstance(
                ServiceLayerDependencyRegistration.DefaultDataStorageDirectoryKey,
                new DirectoryInfo(HostingEnvironment.MapPath(dataStorageDir)));

            string mediaStorageDir = ConfigurationManager.AppSettings[ServiceLayerDependencyRegistration.DefaultMediaStorageDirectoryKey];
            container.RegisterInstance(
                ServiceLayerDependencyRegistration.DefaultMediaStorageDirectoryKey,
                new DirectoryInfo(HostingEnvironment.MapPath(mediaStorageDir)));

            container.RegisterType<BgcUser>(new InjectionFactory(c => c.Resolve<BgcUserManager>().FindByName(HttpContext.Current.User.Identity.Name)));
            
            container.RegisterType<SignInManager<BgcUser, long>>(new InjectionFactory(c =>
            {
                return new SignInManager<BgcUser, long>(c.Resolve<BgcUserManager>(), HttpContext.Current.GetOwinContext().Authentication);
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
