using BGC.Core;
using BGC.Core.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
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
			helper.RegisterType<IComposerEntriesService, ComposerEntriesService>(new InjectionFactory(
				container =>
				{
					ComposersDbContext context = new ComposersDbContext();
					IRepository<Composer> repository = new MySqlRepository<Composer>(context);
					return new ComposerEntriesService(repository);
				}));
		}

		public void RegisterIdentityStores(IUnityContainer helper)
		{
			helper.RegisterType<IUserStore<AspNetUser, long>, UserStore<AspNetUser, AspNetRole, long, AspNetUserLogin, AspNetUserRole, AspNetUserClaim>>(
				new InjectionFactory(container =>
				{
					ComposersDbContext context = container.Resolve<ComposersDbContext>();
					return new UserStore<AspNetUser, AspNetRole, long, AspNetUserLogin, AspNetUserRole, AspNetUserClaim>(context);
				}));
		}
	}
}
