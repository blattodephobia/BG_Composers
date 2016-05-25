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
	public class DataLayerDependencyRegistration : IDependencyRegistration<IUnityContainer>
	{
		public void RegisterTypes(IUnityContainer helper)
		{
            helper.RegisterType(typeof(IRepository<>), typeof(MySqlRepository<>));
			helper.RegisterType<IUnitOfWork, ComposersDbContext>(new InjectionConstructor());
			helper.RegisterType<IUserStore<BgcUser, long>, UserStore<BgcUser, BgcRole, long, BgcUserLogin, BgcUserRole, BgcUserClaim>>(
				new InjectionFactory(container =>
				{
					ComposersDbContext context = container.Resolve<ComposersDbContext>();
					return new UserStore<BgcUser, BgcRole, long, BgcUserLogin, BgcUserRole, BgcUserClaim>(context);
				}));
		}
	}
}
