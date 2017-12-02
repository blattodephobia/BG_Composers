using System;
using System.Collections.Generic;
using System.Globalization;

namespace BGC.Core
{
    public partial class SettingsFactory
    {
        private static readonly IReadOnlyDictionary<Type, Func<string, Setting>> SettingsMap = new Dictionary<Type, Func<string, Setting>>()
        {
 { typeof(CultureInfo), (string name) => new CultureInfoSetting(name) },
 { typeof(DateTime), (string name) => new DateTimeSetting(name) },
 { typeof(IEnumerable<CultureInfo>), (string name) => new MultiCultureInfoSetting(name) },
 { typeof(string), (string name) => new Setting(name) },
        };
    }
}

