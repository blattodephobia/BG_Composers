﻿// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
#pragma warning disable 1591, 3008, 3009, 0108
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;

[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
public static partial class MVC
{
	internal static bool Debugging
	{
		get
		{
		#if DEBUG
			return true;
		#else
			return false;
		#endif
		}
	}
    static readonly AdministrationClass s_Administration = new AdministrationClass();
    public static AdministrationClass AdministrationArea { get { return s_Administration; } }
    static readonly PublicClass s_Public = new PublicClass();
    public static PublicClass Public { get { return s_Public; } }
    public static T4MVC.AdministrationController Administration = new T4MVC.AdministrationController();
    public static T4MVC.SharedController Shared = new T4MVC.SharedController();
}

namespace T4MVC
{
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class AdministrationClass
    {
        public readonly string Name = "Administration";
        public BGC.Web.Areas.Administration.Controllers.AccountController Account = new BGC.Web.Areas.Administration.Controllers.T4MVC_AccountController();
        public BGC.Web.Areas.Administration.Controllers.AuthenticationController Authentication = new BGC.Web.Areas.Administration.Controllers.T4MVC_AuthenticationController();
        public BGC.Web.Areas.Administration.Controllers.EditController Edit = new BGC.Web.Areas.Administration.Controllers.T4MVC_EditController();
        public T4MVC.Administration.SharedController Shared = new T4MVC.Administration.SharedController();
    }
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class PublicClass
    {
        public readonly string Name = "Public";
        public BGC.Web.Areas.Public.Controllers.MainController Main = new BGC.Web.Areas.Public.Controllers.T4MVC_MainController();
        public BGC.Web.Areas.Public.Controllers.ResourcesController Resources = new BGC.Web.Areas.Public.Controllers.T4MVC_ResourcesController();
    }
}

namespace T4MVC
{
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class Dummy
    {
        private Dummy() { }
        public static Dummy Instance = new Dummy();
    }
}

[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
internal partial class T4MVC_System_Web_Mvc_ActionResult : System.Web.Mvc.ActionResult, IT4MVCActionResult
{
    public T4MVC_System_Web_Mvc_ActionResult(string area, string controller, string action, string protocol = null): base()
    {
        this.InitMVCT4Result(area, controller, action, protocol);
    }
     
    public override void ExecuteResult(System.Web.Mvc.ControllerContext context) { }
    
    public string Controller { get; set; }
    public string Action { get; set; }
    public string Protocol { get; set; }
    public RouteValueDictionary RouteValueDictionary { get; set; }
}



namespace Links
{
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static class Scripts {
        private const string URLPATH = "~/Scripts";
        public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
        public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static class Content {
        private const string URLPATH = "~/Content";
        public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
        public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
        public static readonly string Site_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/Site.min.css") ? Url("Site.min.css") : Url("Site.css");
             
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class themes {
            private const string URLPATH = "~/Content/themes";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
            public static class @base {
                private const string URLPATH = "~/Content/themes/base";
                public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
                public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
                [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
                public static class images {
                    private const string URLPATH = "~/Content/themes/base/images";
                    public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
                    public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
                    public static readonly string ui_bg_flat_0_aaaaaa_40x100_png = Url("ui-bg_flat_0_aaaaaa_40x100.png");
                    public static readonly string ui_bg_flat_75_ffffff_40x100_png = Url("ui-bg_flat_75_ffffff_40x100.png");
                    public static readonly string ui_bg_glass_55_fbf9ee_1x400_png = Url("ui-bg_glass_55_fbf9ee_1x400.png");
                    public static readonly string ui_bg_glass_65_ffffff_1x400_png = Url("ui-bg_glass_65_ffffff_1x400.png");
                    public static readonly string ui_bg_glass_75_dadada_1x400_png = Url("ui-bg_glass_75_dadada_1x400.png");
                    public static readonly string ui_bg_glass_75_e6e6e6_1x400_png = Url("ui-bg_glass_75_e6e6e6_1x400.png");
                    public static readonly string ui_bg_glass_95_fef1ec_1x400_png = Url("ui-bg_glass_95_fef1ec_1x400.png");
                    public static readonly string ui_bg_highlight_soft_75_cccccc_1x100_png = Url("ui-bg_highlight-soft_75_cccccc_1x100.png");
                    public static readonly string ui_icons_222222_256x240_png = Url("ui-icons_222222_256x240.png");
                    public static readonly string ui_icons_2e83ff_256x240_png = Url("ui-icons_2e83ff_256x240.png");
                    public static readonly string ui_icons_454545_256x240_png = Url("ui-icons_454545_256x240.png");
                    public static readonly string ui_icons_888888_256x240_png = Url("ui-icons_888888_256x240.png");
                    public static readonly string ui_icons_cd0a0a_256x240_png = Url("ui-icons_cd0a0a_256x240.png");
                }
            
                public static readonly string jquery_ui_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery-ui.min.css") ? Url("jquery-ui.min.css") : Url("jquery-ui.css");
                     
                public static readonly string jquery_ui_accordion_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.ui.accordion.min.css") ? Url("jquery.ui.accordion.min.css") : Url("jquery.ui.accordion.css");
                     
                public static readonly string jquery_ui_all_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.ui.all.min.css") ? Url("jquery.ui.all.min.css") : Url("jquery.ui.all.css");
                     
                public static readonly string jquery_ui_autocomplete_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.ui.autocomplete.min.css") ? Url("jquery.ui.autocomplete.min.css") : Url("jquery.ui.autocomplete.css");
                     
                public static readonly string jquery_ui_base_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.ui.base.min.css") ? Url("jquery.ui.base.min.css") : Url("jquery.ui.base.css");
                     
                public static readonly string jquery_ui_button_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.ui.button.min.css") ? Url("jquery.ui.button.min.css") : Url("jquery.ui.button.css");
                     
                public static readonly string jquery_ui_core_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.ui.core.min.css") ? Url("jquery.ui.core.min.css") : Url("jquery.ui.core.css");
                     
                public static readonly string jquery_ui_datepicker_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.ui.datepicker.min.css") ? Url("jquery.ui.datepicker.min.css") : Url("jquery.ui.datepicker.css");
                     
                public static readonly string jquery_ui_dialog_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.ui.dialog.min.css") ? Url("jquery.ui.dialog.min.css") : Url("jquery.ui.dialog.css");
                     
                public static readonly string jquery_ui_progressbar_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.ui.progressbar.min.css") ? Url("jquery.ui.progressbar.min.css") : Url("jquery.ui.progressbar.css");
                     
                public static readonly string jquery_ui_resizable_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.ui.resizable.min.css") ? Url("jquery.ui.resizable.min.css") : Url("jquery.ui.resizable.css");
                     
                public static readonly string jquery_ui_selectable_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.ui.selectable.min.css") ? Url("jquery.ui.selectable.min.css") : Url("jquery.ui.selectable.css");
                     
                public static readonly string jquery_ui_slider_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.ui.slider.min.css") ? Url("jquery.ui.slider.min.css") : Url("jquery.ui.slider.css");
                     
                public static readonly string jquery_ui_tabs_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.ui.tabs.min.css") ? Url("jquery.ui.tabs.min.css") : Url("jquery.ui.tabs.css");
                     
                public static readonly string jquery_ui_theme_css = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/jquery.ui.theme.min.css") ? Url("jquery.ui.theme.min.css") : Url("jquery.ui.theme.css");
                     
                [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
                public static class minified {
                    private const string URLPATH = "~/Content/themes/base/minified";
                    public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
                    public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
                    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
                    public static class images {
                        private const string URLPATH = "~/Content/themes/base/minified/images";
                        public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
                        public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
                        public static readonly string ui_bg_flat_0_aaaaaa_40x100_png = Url("ui-bg_flat_0_aaaaaa_40x100.png");
                        public static readonly string ui_bg_flat_75_ffffff_40x100_png = Url("ui-bg_flat_75_ffffff_40x100.png");
                        public static readonly string ui_bg_glass_55_fbf9ee_1x400_png = Url("ui-bg_glass_55_fbf9ee_1x400.png");
                        public static readonly string ui_bg_glass_65_ffffff_1x400_png = Url("ui-bg_glass_65_ffffff_1x400.png");
                        public static readonly string ui_bg_glass_75_dadada_1x400_png = Url("ui-bg_glass_75_dadada_1x400.png");
                        public static readonly string ui_bg_glass_75_e6e6e6_1x400_png = Url("ui-bg_glass_75_e6e6e6_1x400.png");
                        public static readonly string ui_bg_glass_95_fef1ec_1x400_png = Url("ui-bg_glass_95_fef1ec_1x400.png");
                        public static readonly string ui_bg_highlight_soft_75_cccccc_1x100_png = Url("ui-bg_highlight-soft_75_cccccc_1x100.png");
                        public static readonly string ui_icons_222222_256x240_png = Url("ui-icons_222222_256x240.png");
                        public static readonly string ui_icons_2e83ff_256x240_png = Url("ui-icons_2e83ff_256x240.png");
                        public static readonly string ui_icons_454545_256x240_png = Url("ui-icons_454545_256x240.png");
                        public static readonly string ui_icons_888888_256x240_png = Url("ui-icons_888888_256x240.png");
                        public static readonly string ui_icons_cd0a0a_256x240_png = Url("ui-icons_cd0a0a_256x240.png");
                    }
                
                    public static readonly string jquery_ui_min_css = Url("jquery-ui.min.css");
                    public static readonly string jquery_ui_accordion_min_css = Url("jquery.ui.accordion.min.css");
                    public static readonly string jquery_ui_autocomplete_min_css = Url("jquery.ui.autocomplete.min.css");
                    public static readonly string jquery_ui_button_min_css = Url("jquery.ui.button.min.css");
                    public static readonly string jquery_ui_core_min_css = Url("jquery.ui.core.min.css");
                    public static readonly string jquery_ui_datepicker_min_css = Url("jquery.ui.datepicker.min.css");
                    public static readonly string jquery_ui_dialog_min_css = Url("jquery.ui.dialog.min.css");
                    public static readonly string jquery_ui_progressbar_min_css = Url("jquery.ui.progressbar.min.css");
                    public static readonly string jquery_ui_resizable_min_css = Url("jquery.ui.resizable.min.css");
                    public static readonly string jquery_ui_selectable_min_css = Url("jquery.ui.selectable.min.css");
                    public static readonly string jquery_ui_slider_min_css = Url("jquery.ui.slider.min.css");
                    public static readonly string jquery_ui_tabs_min_css = Url("jquery.ui.tabs.min.css");
                    public static readonly string jquery_ui_theme_min_css = Url("jquery.ui.theme.min.css");
                }
            
            }
        
        }
    
    }


    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static partial class Areas {
        private const string URLPATH = "~/Areas";
        public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
        public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
    
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static partial class Administration {
            private const string URLPATH = "~/Areas/Administration";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
            public static class Scripts {
                private const string URLPATH = "~/Areas/Administration/Scripts";
                public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
                public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
                [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
                public static class Edit {
                    private const string URLPATH = "~/Areas/Administration/Scripts/Edit";
                    public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
                    public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
                    public static readonly string Add_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/Add.min.js") ? Url("Add.min.js") : Url("Add.js");
                }
            
            }
        
        }
    }

    public static partial class Areas {
    
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static partial class Public {
            private const string URLPATH = "~/Areas/Public";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
            public static class Scripts {
                private const string URLPATH = "~/Areas/Public/Scripts";
                public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
                public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            }
        
        }
    }
    
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static partial class Bundles
    {
        public static partial class Scripts 
        {
            public static class Assets
            {
            }
        }
        public static partial class Content 
        {
            public static partial class themes 
            {
                public static partial class @base 
                {
                    public static partial class images 
                    {
                        public static class Assets
                        {
                        }
                    }
                    public static partial class minified 
                    {
                        public static partial class images 
                        {
                            public static class Assets
                            {
                            }
                        }
                        public static class Assets
                        {
                            public const string jquery_ui_min_css = "~/Content/themes/base/minified/jquery-ui.min.css";
                            public const string jquery_ui_accordion_min_css = "~/Content/themes/base/minified/jquery.ui.accordion.min.css";
                            public const string jquery_ui_autocomplete_min_css = "~/Content/themes/base/minified/jquery.ui.autocomplete.min.css";
                            public const string jquery_ui_button_min_css = "~/Content/themes/base/minified/jquery.ui.button.min.css";
                            public const string jquery_ui_core_min_css = "~/Content/themes/base/minified/jquery.ui.core.min.css";
                            public const string jquery_ui_datepicker_min_css = "~/Content/themes/base/minified/jquery.ui.datepicker.min.css";
                            public const string jquery_ui_dialog_min_css = "~/Content/themes/base/minified/jquery.ui.dialog.min.css";
                            public const string jquery_ui_progressbar_min_css = "~/Content/themes/base/minified/jquery.ui.progressbar.min.css";
                            public const string jquery_ui_resizable_min_css = "~/Content/themes/base/minified/jquery.ui.resizable.min.css";
                            public const string jquery_ui_selectable_min_css = "~/Content/themes/base/minified/jquery.ui.selectable.min.css";
                            public const string jquery_ui_slider_min_css = "~/Content/themes/base/minified/jquery.ui.slider.min.css";
                            public const string jquery_ui_tabs_min_css = "~/Content/themes/base/minified/jquery.ui.tabs.min.css";
                            public const string jquery_ui_theme_min_css = "~/Content/themes/base/minified/jquery.ui.theme.min.css";
                        }
                    }
                    public static class Assets
                    {
                        public const string jquery_ui_css = "~/Content/themes/base/jquery-ui.css";
                        public const string jquery_ui_accordion_css = "~/Content/themes/base/jquery.ui.accordion.css";
                        public const string jquery_ui_all_css = "~/Content/themes/base/jquery.ui.all.css";
                        public const string jquery_ui_autocomplete_css = "~/Content/themes/base/jquery.ui.autocomplete.css";
                        public const string jquery_ui_base_css = "~/Content/themes/base/jquery.ui.base.css";
                        public const string jquery_ui_button_css = "~/Content/themes/base/jquery.ui.button.css";
                        public const string jquery_ui_core_css = "~/Content/themes/base/jquery.ui.core.css";
                        public const string jquery_ui_datepicker_css = "~/Content/themes/base/jquery.ui.datepicker.css";
                        public const string jquery_ui_dialog_css = "~/Content/themes/base/jquery.ui.dialog.css";
                        public const string jquery_ui_progressbar_css = "~/Content/themes/base/jquery.ui.progressbar.css";
                        public const string jquery_ui_resizable_css = "~/Content/themes/base/jquery.ui.resizable.css";
                        public const string jquery_ui_selectable_css = "~/Content/themes/base/jquery.ui.selectable.css";
                        public const string jquery_ui_slider_css = "~/Content/themes/base/jquery.ui.slider.css";
                        public const string jquery_ui_tabs_css = "~/Content/themes/base/jquery.ui.tabs.css";
                        public const string jquery_ui_theme_css = "~/Content/themes/base/jquery.ui.theme.css";
                    }
                }
                public static class Assets
                {
                }
            }
            public static class Assets
            {
                public const string Site_css = "~/Content/Site.css";
            }
        }
    }
}

[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
internal static class T4MVCHelpers {
    // You can change the ProcessVirtualPath method to modify the path that gets returned to the client.
    // e.g. you can prepend a domain, or append a query string:
    //      return "http://localhost" + path + "?foo=bar";
    private static string ProcessVirtualPathDefault(string virtualPath) {
        // The path that comes in starts with ~/ and must first be made absolute
        string path = VirtualPathUtility.ToAbsolute(virtualPath);
        
        // Add your own modifications here before returning the path
        return path;
    }

    // Calling ProcessVirtualPath through delegate to allow it to be replaced for unit testing
    public static Func<string, string> ProcessVirtualPath = ProcessVirtualPathDefault;

    // Calling T4Extension.TimestampString through delegate to allow it to be replaced for unit testing and other purposes
    public static Func<string, string> TimestampString = System.Web.Mvc.T4Extensions.TimestampString;

    // Logic to determine if the app is running in production or dev environment
    public static bool IsProduction() { 
        return (HttpContext.Current != null && !HttpContext.Current.IsDebuggingEnabled); 
    }
}





#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108


