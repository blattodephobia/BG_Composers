using BGC.Core.Services;
using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class ConfigurationBase
    {
        private ISettingsService _svc;
        private Dictionary<string, Setting> _cache = new Dictionary<string, Setting>();
        private string _prefix;
        private SettingsFactory _factory;
        private Dictionary<string, Type> _expectedSettingTypes;

        private bool IsFullSettingName(string name)
        {
            int prefixIndex = name.IndexOf(_prefix);
            if (prefixIndex >= 0)
            {
                return !name.Substring(prefixIndex + _prefix.Length + 1).Any(@char => @char == '.');
            }
            else
            {
                return false;
            }
        }

        private string GetFullSettingName(string name) =>
            string.IsNullOrWhiteSpace(_prefix) || IsFullSettingName(name)
            ? name
            : $"{_prefix}.{name}";

        private void Cache(string settingName)
        {
            settingName.ArgumentNotNull().ThrowOnError();

            if (!_cache.ContainsKey(settingName))
            {
                Shield.Assert(settingName, _expectedSettingTypes.ContainsKey(settingName), x => new SettingException($"The setting '{settingName}' is not recognized as a valid property of this class.")).ThrowOnError();

                Type expectedType = _expectedSettingTypes[settingName];
                Setting setting = _svc.ReadSetting(settingName);
                string origin = _svc.GetType().FullName;
                if (setting == null)
                {
                    setting = _factory.GetSetting(settingName, expectedType);
                    origin = _factory.GetType().FullName;
                }

                Shield.Assert(setting, setting.ValueType == expectedType, x =>
                {
                    var exception = new SettingTypeMismatchException(settingName, expectedType, x.ValueType);
                    exception.Data.Add(nameof(origin), origin);
                    return exception;
                }).ThrowOnError();

                _svc.WriteSetting(setting);
                _cache.Add(settingName, setting);
            }
        }

        private IParameter<T> GetSetting<T>(string name) => _cache[GetFullSettingName(name)] as IParameter<T>;

        public void SetValue<T>(T value, [CallerMemberName] string settingName = null)
        {
            string fullSettingName = GetFullSettingName(settingName);
            Cache(fullSettingName);

            Setting setting = _cache[fullSettingName];
            (setting as IParameter<T>).Value = value;
            _svc.WriteSetting(setting);
        }

        public void SetValue(string value, string settingName)
        {
            string fullSettingName = GetFullSettingName(settingName);
            Cache(fullSettingName);

            Setting setting = _cache[fullSettingName];
            setting.StringValue = value;
            _svc.WriteSetting(setting);
        }

        public T ReadValue<T>([CallerMemberName] string settingName = null)
        {
            string fullSettingName = GetFullSettingName(settingName);
            Cache(fullSettingName);

            IParameter<T> result = GetSetting<T>(settingName);
            return result.Value;
        }

        public IEnumerable<Setting> AllSettings()
        {
            var settingNames = from property in GetType().GetProperties()
                               let fullSettingName = GetFullSettingName(property.Name)
                               let expectedType = property.PropertyType
                               select new { fullSettingName, expectedType };

            List<Setting> result = new List<Setting>(GetType().GetProperties().Length);
            foreach (var settingInfo in settingNames)
            {
                Type expectedType = settingInfo.expectedType;
                Setting setting = null;
                if (!_cache.ContainsKey(settingInfo.fullSettingName))
                {
                    setting = _svc.ReadSetting(settingInfo.fullSettingName);
                    if (setting == null)
                    {
                        setting = (_factory as IServiceProvider).GetService(settingInfo.expectedType) as Setting;
                        setting.Name = settingInfo.fullSettingName;
                    }
                    _cache.Add(settingInfo.fullSettingName, setting);
                }
                else
                {
                    setting = _cache[settingInfo.fullSettingName];
                }

                Shield.Assert(setting, setting.ValueType == expectedType, x => new SettingTypeMismatchException(setting.Name, expectedType, setting.ValueType)).ThrowOnError();

                result.Add(setting);
            }

            return result;
        }

        public string GetDescriptionFor(string settingName) => GetType().GetProperty(settingName)?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";

        public ConfigurationBase(ISettingsService settingsService, string implicitSettingsNamePrefix = null, SettingsFactory factory = null) :
            this(implicitSettingsNamePrefix)
        {
            Shield.ArgumentNotNull(settingsService, nameof(settingsService)).ThrowOnError();

            _svc = settingsService;
            _factory = factory ?? new SettingsFactory();
        }

        private ConfigurationBase(string implicitSettingsNamePrefix = null)
        {
            _prefix = implicitSettingsNamePrefix;
            _expectedSettingTypes = GetType().GetProperties().ToDictionary(p => GetFullSettingName(p.Name), p => p.PropertyType);
        }
    }
}
