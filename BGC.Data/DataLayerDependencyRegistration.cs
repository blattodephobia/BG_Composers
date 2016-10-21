using BGC.Core;
using BGC.Core.Services;
using CodeShield;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data
{
	public class DataLayerDependencyRegistration : IDependencyRegistration<IUnityContainer, LifetimeManager>
	{
        private static readonly Dictionary<Type, Action<IUnityContainer, LifetimeManager>> RegistrationDelegates = new Dictionary<Type, Action<IUnityContainer, LifetimeManager>>()
        {
            { typeof(IRepository<>),             (c, lm) => c.RegisterType(typeof(IRepository<>), typeof(MySqlRepository<>), lm) },
            { typeof(IUnitOfWork),               (c, lm) => c.RegisterType<IUnitOfWork, ComposersDbContext>(lm, new InjectionConstructor()) },
            { typeof(DbContext),                 (c, lm) => c.RegisterType<DbContext, ComposersDbContext>(lm, new InjectionConstructor()) },
            { typeof(IRoleStore<BgcRole, long>), (c, lm) => c.RegisterType<IRoleStore<BgcRole, long>, RoleStore<BgcRole, long, BgcUserRole>>() },
            { typeof(IUserStore<BgcUser, long>), (c, lm) => c.RegisterType<IUserStore<BgcUser, long>, UserStore<BgcUser, BgcRole, long, BgcUserLogin, BgcUserRole, BgcUserClaim>>(lm, new InjectionFactory(container =>
				{
					ComposersDbContext context = container.Resolve<ComposersDbContext>();
					return new UserStore<BgcUser, BgcRole, long, BgcUserLogin, BgcUserRole, BgcUserClaim>(context);
				}))
            }
        };

        private readonly IUnityContainer container;

        public DataLayerDependencyRegistration()
        {
        }

        public DataLayerDependencyRegistration(IUnityContainer container)
        {
            this.container = Shield.ArgumentNotNull(container).GetValueOrThrow();
        }

        public void RegisterType(Type type, LifetimeManager scope = null)
        {
            Shield.AssertOperation(
                this.container,
                c => c != null,
                $"This overload of {nameof(RegisterType)} requires that the current instance of {nameof(DataLayerDependencyRegistration)} has been initialized with a valid instance of {nameof(IUnityContainer)}.").ThrowOnError();
            
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
