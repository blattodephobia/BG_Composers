using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Public
{
	public class PublicAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get { return MVC.Public.Name; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context
                .Routes
				.MapRoute(
					name: "Standard",
					url: "{controller}/{action}")
				.DataTokens.Add("area", AreaName);

            context
                .Routes
                .MapRoute(
                    name: "HomePage",
                    url: "",
                    defaults: new { controller = MVC.Public.Main.Name, action = MVC.Public.Main.ActionNames.Index })
                .DataTokens.Add("area", AreaName);
        }
	}
}