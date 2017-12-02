using BGC.Core.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;
using static TestUtils.MockUtilities;

namespace BGC.Core.Tests.Models.Settings.ConfigurationBaseTests
{
    public class ReadSettingTests : TestFixtureBase
    {
        private class ConfigurationBaseProxy : ConfigurationBase
        {
            public ConfigurationBaseProxy(ISettingsService svc, string settingsPrefix, SettingsFactory factory = null) :
                base(svc, settingsPrefix, factory)
            {
            }

            public new T ReadValue<T>(string name) => base.ReadValue<T>(name);

            public new void SetValue<T>(T value, string name) => base.SetValue(value, name);

            public object MissingConfigurationPropertySetting
            {
                get
                {
                    return ReadValue<object>();
                }

                set
                {
                    SetValue(value);
                }
            }

            public CultureInfo SupportedType
            {
                get
                {
                    return ReadValue<CultureInfo>();
                }

                set
                {
                    SetValue(value);
                }
            }
        }

        [Test]
        public void ReadsSetting()
        {
            ConfigurationBaseProxy config = new ConfigurationBaseProxy(GetMockSettingsService().Object, null);
            string cultureCode = "de-DE";
            config.SupportedType = new CultureInfo(cultureCode);

            Assert.AreEqual(cultureCode, config.SupportedType.Name);
        }

        [Test]
        public void CreatesSettingInLazyManner()
        {
            List<Setting> settingsRepo = new List<Setting>();
            Mock<ISettingsService> svc = GetMockSettingsService(settingsRepo);
            ConfigurationBaseProxy config = new ConfigurationBaseProxy(svc.Object, null);

            config.SupportedType = new CultureInfo("bg-BG");

            Assert.IsNotNull(settingsRepo.FirstOrDefault(s => s.Name == nameof(config.SupportedType)));
        }

        [Test]
        public void ThrowsExceptionIfNoStronglyTypedConversionAvailable()
        {
            List<Setting> settingsRepo = new List<Setting>();
            Mock<ISettingsService> faultySettingsService = GetMockSettingsService(settingsRepo);
            faultySettingsService.Setup(x => x.ReadSetting(It.IsAny<string>())).Returns(new Setting("wrong"));

            ConfigurationBaseProxy config = new ConfigurationBaseProxy(faultySettingsService.Object, null);

            Assert.Throws<InvalidOperationException>(() =>
            {
                config.SupportedType = new CultureInfo("jp-JP");
            });
        }
    }
}
