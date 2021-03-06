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
using BGC.Web.Services;
using BGC.Web.Models;

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
        public static void RegisterTypes(UnityContainer container)
        {
            var dataLayerDependencyRegistration = new DataLayerDependencyRegistration(container);
            dataLayerDependencyRegistration.RegisterType(typeof(IRepository<>));
            dataLayerDependencyRegistration.RegisterType(typeof(IComposerRepository));
            dataLayerDependencyRegistration.RegisterType(typeof(DbContext), new PerRequestLifetimeManager());
            dataLayerDependencyRegistration.RegisterType(typeof(IUnitOfWork), new PerRequestLifetimeManager());
            dataLayerDependencyRegistration.RegisterType(typeof(IUserStore<BgcUser, long>));
            dataLayerDependencyRegistration.RegisterType(typeof(IRoleStore<BgcRole, long>));
            dataLayerDependencyRegistration.RegisterType(typeof(INonQueryableRepository<Guid, MediaTypeInfo>));

            var serviceLayerDependencyRegistration = new ServiceLayerDependencyRegistration(container);
            serviceLayerDependencyRegistration.RegisterType(typeof(IComposerDataService),   container);
            serviceLayerDependencyRegistration.RegisterType(typeof(ISettingsService),       container);
            serviceLayerDependencyRegistration.RegisterType(typeof(IArticleContentService), container);
            serviceLayerDependencyRegistration.RegisterType(typeof(IMediaService),          container);
            serviceLayerDependencyRegistration.RegisterType(typeof(IGlossaryService),       container);

            container.RegisterType<ISearchService>(nameof(Composer), new InjectionFactory(c => c.Resolve<IComposerDataService>()));

            string dataStorageDir = ConfigurationManager.AppSettings[ServiceLayerDependencyRegistration.DefaultDataStorageDirectoryKey];
            container.RegisterInstance(
                ServiceLayerDependencyRegistration.DefaultDataStorageDirectoryKey,
                new DirectoryInfo(HostingEnvironment.MapPath(dataStorageDir)));

            string mediaStorageDir = ConfigurationManager.AppSettings[ServiceLayerDependencyRegistration.DefaultMediaStorageDirectoryKey];
            container.RegisterInstance(
                ServiceLayerDependencyRegistration.DefaultMediaStorageDirectoryKey,
                new DirectoryInfo(HostingEnvironment.MapPath(mediaStorageDir)));

            container.RegisterType(typeof(BgcUserManager),
                new InjectionProperty(nameof(BgcUserManager.EmailService), new EmailService("SystemEmails")),
                new InjectionProperty(nameof(BgcUserManager.UserTokenProvider), new BgcUserTokenProvider()));

            container.RegisterType<BgcUser>(new InjectionFactory(c => c.Resolve<BgcUserManager>().FindByName(HttpContext.Current.User.Identity.Name)));
            
            container.RegisterType<SignInManager<BgcUser, long>>(new InjectionFactory(c =>
            {
                return new BgcSignInManager(c.Resolve<BgcUserManager>(), HttpContext.Current.GetOwinContext().Authentication);
            }));

            container.RegisterType<ILocalizationService, LocalizationService>
            (
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(c =>
                {
                    string xmlPath = $@"{HostingEnvironment.ApplicationPhysicalPath}\Localization\Localization.xml";
                    using (Stream xmlStream = File.OpenRead(xmlPath))
                    {
                        XmlDocument localizationXml = new XmlDocument();
                        localizationXml.Load(xmlStream);
                        return new LocalizationService(localizationXml);
                    }
                })
            );

            IUnityContainer tempContainer = new UnityContainer();
            dataLayerDependencyRegistration.RegisterType(typeof(DbContext), tempContainer, new PerResolveLifetimeManager());
            dataLayerDependencyRegistration.RegisterType(typeof(IRepository<>), tempContainer, new PerResolveLifetimeManager());
            dataLayerDependencyRegistration.RegisterType(typeof(IUnitOfWork), tempContainer, new PerResolveLifetimeManager());
            WebApplicationSettings profile = WebApplicationSettings.FromApplicationSettings(tempContainer.Resolve<IRepository<Setting>>().All());
            profile.LocaleCookieName = "locale";
            profile.LocaleRouteTokenName = "locale";
            profile.LocaleKey = "locale";
            container.RegisterInstance(profile);

            container.RegisterInstance<IGeoLocationService>(new DynamicMaxMindServiceProvider(HttpRuntime.AppDomainAppPath + @"App_Data\Geolocation\GeoLite2-Country.mmdb"));
        }
    }
}
