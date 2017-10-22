using BGC.Core.Services;
using BGC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    /// <summary>
    /// Contains the names, descriptions and types of the available system settings. The actual instances and their values should
    /// be obtained through the <see cref="ISettingsService"/>.
    /// </summary>
    public class SystemSettings
    {
        public MultiCultureInfoSetting AvailableCultures { get; private set; } = new MultiCultureInfoSetting(nameof(AvailableCultures))
        {
            Description = "The languages the encyclopedia can be displayed in."
        };

        public MultiCultureInfoSetting SelectedCultures { get; private set; } = new MultiCultureInfoSetting(nameof(SelectedCultures))
        {
            Description = "The languages the encyclopedia will be available in.",
        };

        public Setting EmailInvitationMessage { get; private set; } = new Setting(nameof(EmailInvitationMessage))
        {
            Description = "The content of the email message that will be sent to invited users.",
        };

        public IReadOnlyList<Setting> All { get; private set; }

        public SystemSettings()
        {
            All = Array.AsReadOnly(Expressions.GetPropertyValuesOfTypeAccessor<Setting>(typeof(SystemSettings)).Invoke(null).ToArray());
            foreach (Setting setting in All)
            {
                setting.Priority = SettingPriority.Application;
                setting.SetReadOnly();
            }
        }
    }
}
