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
    internal class SettingsService : DbServiceBase, ISettingsService, IComparer<Setting>
    {
        private static readonly Dictionary<Type, int> SettingPriorities = new Dictionary<Type, int>()
        {
            { typeof(ApplicationSetting), 0 },
            { typeof(UserSetting), 1 },
        };

        private IRepository<ApplicationSetting> AppSettings { get; set; }
        private IRepository<UserSetting> UserSettings { get; set; }

        public SettingsService(IRepository<ApplicationSetting> appSettings, IRepository<UserSetting> userSettings = null)
        {
            Shield.ArgumentNotNull(appSettings, nameof(appSettings)).ThrowOnError();

            AppSettings = appSettings;
            UserSettings = userSettings;
        }

        public Setting FindSetting(string name)
        {
            List<Setting> matchingSettings = new List<Setting>();
            matchingSettings.AddRange(AppSettings.All().Where(s => s.Name == name));
            matchingSettings.AddRange(UserSettings.All().Where(s => s.Name == name));

            return matchingSettings.OrderBy(setting => setting, this).LastOrDefault();
        }

        int IComparer<Setting>.Compare(Setting x, Setting y)
        {
            Shield
                .ArgumentNotNull(x, nameof(x))
                .And(Shield.ArgumentNotNull(y, nameof(y)))
                .ThrowOnError();

            int xPriority;
            int yPriority;

            if (SettingPriorities.TryGetValue(x.GetType(), out xPriority) &&
                SettingPriorities.TryGetValue(x.GetType(), out yPriority))
            {
                return xPriority.CompareTo(yPriority);
            }
            else
            {
                throw new InvalidOperationException($"One or more of the given setting types for priority comparison are invalid: {x.GetType()}, {y.GetType()}.");
            }
        }
    }
}