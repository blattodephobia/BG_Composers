using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Services
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Finds the highest priority setting that the service can currently access by using the given setting's name and attempts to cast
        /// it to its actual derived type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T ReadSetting<T>(this ISettingsService settingsService, string name) where T : Setting
        {
            Shield.ArgumentNotNull(settingsService, nameof(settingsService)).ThrowOnError();
            Shield.IsNotNullOrEmpty(name, nameof(settingsService)).ThrowOnError();

            Setting actualSetting = settingsService.ReadSetting(name);
            return actualSetting == null
                ? null
                : (T)actualSetting;
        }
    }
}
