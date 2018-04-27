using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Web;

namespace BGC.Core
{
    public partial class SettingsFactory
    {
        private static readonly IReadOnlyDictionary<Type, Func<string, Setting>> SettingsMap = new Dictionary<Type, Func<string, Setting>>()
        {
 { typeof(CultureInfo), (string name) => new CultureInfoSetting(name) },
 { typeof(DateTime), (string name) => new DateTimeSetting(name) },
 { typeof(HtmlString), (string name) => new HtmlEncodedStringSetting(name) },
 { typeof(IEnumerable<CultureInfo>), (string name) => new MultiCultureInfoSetting(name) },
 { typeof(string), (string name) => new Setting(name) },
 { typeof(XmlDocument), (string name) => new XmlDocumentSetting(name) },
        };
    }
}

