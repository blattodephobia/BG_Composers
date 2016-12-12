using BGC.Web.Areas.Administration.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration
{
	public class AdministrationAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get { return MVC.Administration.Name; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
            context
                .Routes
                .MapRoute(
                    name: $"{AreaName}",
                    url: $"{AreaName}/{{controller}}/{{action}}",
                    namespaces: new[] { typeof(AdministrationControllerBase).Namespace },
                    defaults: new { controller = MVC.Administration.Account.Name, action = MVC.Administration.Account.ActionNames.Activities })
                .DataTokens.Add("area", AreaName);
        }
	}
}