using BGC.Core;
using BGC.Core.Services;
using BGC.Web.Areas.Administration.Models;
using BGC.Web.Areas.Administration.ViewModels.Permissions;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.Controllers
{
    public partial class SettingsController : AdministrationControllerBase
    {
        private ISettingsService _settingsService;
        private ApplicationConfiguration _globalSettings;

        public ApplicationConfiguration GlobalSettings => _globalSettings ?? (_globalSettings = new ApplicationConfiguration(_settingsService));

        [Permissions(nameof(IApplicationSettingsWritePermission))]
        public virtual ActionResult ApplicationSettings()
        {
            var appSettings = from setting in GlobalSettings.AllSettings()
                              select new SettingWebModel(setting.Name, setting.ValueType);
            ApplicationSettingsWritePermissionViewModel vm = new ApplicationSettingsWritePermissionViewModel()
            {
                Settings = appSettings.ToList()
            };
            return View(vm);
        }

        [Permissions(nameof(IApplicationSettingsWritePermission))]
        [HttpPost, ActionName(nameof(ApplicationSettings))]
        public virtual ActionResult ApplicationSettings_Post(ApplicationSettingsWritePermissionViewModel vm)
        {
            ApplicationConfiguration config = new ApplicationConfiguration(_settingsService);
            Type configType = typeof(ApplicationConfiguration);

            foreach (SettingWebModel webModelSetting in vm.Settings)
            {
                config.SetValue(webModelSetting.Value, webModelSetting.Name);
            }

            return ApplicationSettings();
        }

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService.ArgumentNotNull(nameof(settingsService)).GetValueOrThrow();
        }
    }
}
