using System.Web;
using System.Web.Optimization;

namespace BGC.Web
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Content/themes/base").Include(
				"~/Content/themes/base/jquery.ui.core.css",
				"~/Content/themes/base/jquery.ui.resizable.css",
				"~/Content/themes/base/jquery.ui.selectable.css",
				"~/Content/themes/base/jquery.ui.accordion.css",
				"~/Content/themes/base/jquery.ui.autocomplete.css",
				"~/Content/themes/base/jquery.ui.button.css",
				"~/Content/themes/base/jquery.ui.dialog.css",
				"~/Content/themes/base/jquery.ui.slider.css",
				"~/Content/themes/base/jquery.ui.tabs.css",
				"~/Content/themes/base/jquery.ui.datepicker.css",
				"~/Content/themes/base/jquery.ui.progressbar.css",
				"~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Areas/Administration/Resources/css").Include(
                "~/Areas/Administration/Resources/css/bt-themes/ace-master/ace.min.css",
                "~/Areas/Administration/Resources/css/bt-themes/ace-master/ace-ie.min.css",
                "~/Areas/Administration/Resources/css/bt-themes/ace-master/ace-part2.min.css",
                "~/Areas/Administration/Resources/css/bt-themes/ace-master/ace-rtl.min.css",
                "~/Areas/Administration/Resources/css/bt-themes/ace-master/ace-skins.min.css"));

        }
	}
}