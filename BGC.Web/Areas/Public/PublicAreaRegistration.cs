using System;
using System.Collections.Generic;
using System.Globalization;
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
                    url: "{locale}",
                    defaults: new { controller = MVC.Public.Main.Name, action = MVC.Public.Main.ActionNames.Index, locale = "en-US" })
                .DataTokens.Add("area", AreaName);
        }
	}
}