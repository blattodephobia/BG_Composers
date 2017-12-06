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

        [Permissions(nameof(IApplicationSettingsWritePermission))]
        public virtual ActionResult ApplicationSettings()
        {
            var appSettings = from property in typeof(ApplicationConfiguration).GetProperties()
                              select new SettingWebModel(property.Name, property.PropertyType);
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
                configType
                    .GetProperty(webModelSetting.Name)
                    .SetValue(config, Convert.ChangeType(webModelSetting.Value, webModelSetting.Type));
            }

            return ApplicationSettings();
        }

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService.ArgumentNotNull(nameof(settingsService)).GetValueOrThrow();
        }
    }
}
