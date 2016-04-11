using System;
using System.Linq;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using BGC.Core;
using BGC.Data;
using BGC.WebAPI.Areas.Administration.Controllers;
using BGC.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web;
using System.Reflection;
using BGC.Web.Views;
using BGC.Core.Services;
using BGC.Services;

namespace BGC.WebAPI.App_Start
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
			new BGC.Data.DataLayerDependencyRegistration().RegisterTypes(container);
			new BGC.Services.ServiceLayerDependencyRegistration().RegisterTypes(container);

			container
				.RegisterType<AdministrationControllerBase>()
				.RegisterTypes(
					types: AllClasses
						   .FromAssemblies(typeof(AdministrationControllerBase).Assembly)
						   .Where(t => typeof(AdministrationControllerBase).IsAssignableFrom(t)),
					getInjectionMembers: (t) => new InjectionMember[]
					{
						new InjectionProperty(
							Expressions.NameOf<AdministrationControllerBase>(obj => obj.UserManager),
							container.Resolve<UserManager<AspNetUser, long>>())
					});

			container.RegisterType<SignInManager<AspNetUser, long>>(new InjectionFactory(c =>
			{
				return new SignInManager<AspNetUser, long>(c.Resolve<UserManager<AspNetUser, long>>(), HttpContext.Current.GetOwinContext().Authentication);
			}));
        }
    }
}
