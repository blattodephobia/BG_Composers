using BGC.Core;
using BGC.Core.Services;
using BGC.Data.Relational;
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
        private static readonly Dictionary<Type, Action<IUnityContainer, LifetimeManager, string>> RegistrationDelegates = new Dictionary<Type, Action<IUnityContainer, LifetimeManager, string>>()
        {
            { typeof(IRepository<>),             (c, lm, name) => c.RegisterType(typeof(IRepository<>), typeof(MySqlRepository<>), name, lm) },
            { typeof(IUnitOfWork),               (c, lm, name) => c.RegisterType<IUnitOfWork, ComposersDbContext>(name, lm, new InjectionConstructor()) },
            { typeof(DbContext),                 (c, lm, name) => c.RegisterType<DbContext, ComposersDbContext>(name, lm, new InjectionConstructor()) },
            { typeof(IRoleStore<BgcRole, long>), (c, lm, name) => c.RegisterType<IRoleStore<BgcRole, long>, RoleStore<BgcRole, long, BgcUserRole>>(name, lm) },
            { typeof(IUserStore<BgcUser, long>), (c, lm, name) => c.RegisterType<IUserStore<BgcUser, long>, UserStore<BgcUser, BgcRole, long, BgcUserLogin, BgcUserRole, BgcUserClaim>>(name, lm, new InjectionFactory(container =>
				{
					ComposersDbContext context = container.Resolve<ComposersDbContext>();
					return new UserStore<BgcUser, BgcRole, long, BgcUserLogin, BgcUserRole, BgcUserClaim>(context);
				}))
            }
        };

        private readonly IUnityContainer _container;

        public DataLayerDependencyRegistration()
        {
        }

        public DataLayerDependencyRegistration(IUnityContainer container)
        {
            _container = Shield.ArgumentNotNull(container).GetValueOrThrow();
        }

        public void RegisterType(Type type, LifetimeManager scope = null)
        {
            Shield.AssertOperation(
                _container,
                c => c != null,
                $"This overload of {nameof(RegisterType)} requires that the current instance of {nameof(DataLayerDependencyRegistration)} has been initialized with a valid instance of {nameof(IUnityContainer)}.").ThrowOnError();
            
            RegisterType(type, _container.AssertOperation(_container != null, nameof(_container)).GetValueOrThrow(), scope);
        }

        public void RegisterType(Type type, IUnityContainer helper, LifetimeManager scope = null)
        {
            Shield.ArgumentNotNull(helper, nameof(helper)).ThrowOnError();
            Shield.ArgumentNotNull(type, nameof(type)).ThrowOnError();
            Shield.AssertOperation(type, t => RegistrationDelegates.ContainsKey(t), $"The type {type.FullName} is not supported by this assembly and cannot be registered.").ThrowOnError();

            RegistrationDelegates[type].Invoke(helper, scope, null);
        }

        public void RegisterType(Type type, IUnityContainer helper, string name, LifetimeManager scope = null)
        {
            Shield.ArgumentNotNull(helper, nameof(helper)).ThrowOnError();
            Shield.ArgumentNotNull(type, nameof(type)).ThrowOnError();
            Shield.AssertOperation(type, t => RegistrationDelegates.ContainsKey(t), $"The type {type.FullName} is not supported by this assembly and cannot be registered.").ThrowOnError();

            RegistrationDelegates[type].Invoke(helper, scope, name);
        }
    }
}
