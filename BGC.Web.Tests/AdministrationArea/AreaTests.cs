using BGC.WebAPI.Areas.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BGC.WebAPI.Tests.AdministrationArea
{
	[TestClass]
	public class AreaTests
	{
		[TestMethod]
		public void AllControllersHaveAuthorizationAttribute()
		{
			IEnumerable<Type> controllers = typeof(AdministrationAreaRegistration).Assembly.GetTypes()
				.Where(t => t.Namespace != null && t.Namespace.StartsWith(typeof(BGC.WebAPI.Areas.Administration.AdministrationAreaRegistration).Namespace))
				.Where(t => typeof(Controller).IsAssignableFrom(t));
			bool allControllersHaveAuthorization = controllers.All(t => t.GetCustomAttributes(true).Any(obj => obj is AuthorizeAttribute));

			Assert.IsTrue(allControllersHaveAuthorization || !controllers.Any());
		}
	}
}
