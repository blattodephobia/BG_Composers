using BGC.Core;
using BGC.Core.Services;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services
{
    internal class SettingsService : DbServiceBase, ISettingsService
    {
        private IRepository<Setting> Settings { get; set; }
        private BgcUser CurrentUser { get; set; }

        public SettingsService(IRepository<Setting> settings, BgcUser currentUser = null)
        {
            Shield.ArgumentNotNull(settings, nameof(settings)).ThrowOnError();

            Settings = settings;
            CurrentUser = currentUser;
        }

        public Setting ReadSetting(string name)
        {
            List<Setting> matchingSettings = new List<Setting>();
            matchingSettings.AddRange(Settings.All().Where(s => s.Name == name));
            matchingSettings.AddRange(CurrentUser?.UserSettings.Where(s => s.Name == name) ?? Enumerable.Empty<Setting>());

            return matchingSettings.OrderBy(setting => (int)setting.Priority).LastOrDefault();
        }

        public T ReadSetting<T>(string name) where T : Setting
        {
            return (T)ReadSetting(name);
        }
    }
}