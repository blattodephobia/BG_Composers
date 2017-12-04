using BGC.Core;
using BGC.Core.Exceptions;
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

        public SettingsService(IRepository<Setting> settings) :
            this(settings, null)
        {
        }

        public SettingsService(IRepository<Setting> settings, BgcUser currentUser)
        {
            Shield.ArgumentNotNull(settings, nameof(settings)).ThrowOnError();

            Settings = settings;
            CurrentUser = currentUser;
        }

        public Setting ReadSetting(string name)
        {
            Shield.IsNotNullOrEmpty(name, nameof(name)).ThrowOnError();

            List<Setting> matchingSettings = new List<Setting>();
            matchingSettings.AddRange(Settings.All().Where(s => s.Name == name && s.Priority == SettingPriority.Application));
            matchingSettings.AddRange(CurrentUser?.UserSettings.Where(s => s.Name == name) ?? Enumerable.Empty<Setting>());

            return matchingSettings.OrderByDescending(setting => (int)setting.Priority).FirstOrDefault();
        }

        public void WriteSetting(Setting setting)
        {
            Shield.ArgumentNotNull(setting).ThrowOnError();

            Setting existingSetting = Settings.All().FirstOrDefault(s => s.Name == setting.Name && s.OwnerStamp == setting.OwnerStamp);
            if (existingSetting != null)
            {
                Shield.Assert(
                    setting,
                    setting.ValueType == existingSetting.ValueType,
                    (Setting s) => new SettingException($"The type of the setting {setting.Name} conflicts with the type of the existing setting bearing the same name.")).ThrowOnError();

                existingSetting.StringValue = setting.StringValue;
            }
            else
            {
                Settings.Insert(setting);
            }

            SaveAll();
        }

        public void DeleteSetting(Setting setting)
        {
            Settings.Delete(setting);

            SaveAll();
        }
    }
}