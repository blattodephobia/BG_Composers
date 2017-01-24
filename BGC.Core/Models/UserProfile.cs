using CodeShield;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class UserProfile
    {
        private static readonly string LocaleSettingName = "Locale";
        private readonly BgcUser _user;

        private CultureInfoSetting FindSetting() => _user.UserSettings?.OfType<CultureInfoSetting>().FirstOrDefault(c => c.Name == LocaleSettingName);

        private void SetLocale(CultureInfo locale)
        {
            CultureInfoSetting setting = FindSetting();
            if (setting == null)
            {
                _user.UserSettings.Add(setting = new CultureInfoSetting(locale) { Name = LocaleSettingName });
                
            }
            else
            {
                setting.Locale = locale;
            }
        }

        public CultureInfo PreferredLocale
        {
            get
            {
                return FindSetting()?.Locale;
            }

            set
            {
                SetLocale(value);
            }
        }

        public UserProfile(BgcUser user)
        {
            Shield.ArgumentNotNull(user).ThrowOnError();

            _user = user;
        }
    }
}
