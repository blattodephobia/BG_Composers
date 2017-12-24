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
                config.SupportedType = new CultureInfo("ja-JP");
            });
        }

        [Test]
        public void SetsAndReadsSettingsWithPrefix()
        {
            List<Setting> settingsRepo = new List<Setting>();
            Mock<ISettingsService> svc = GetMockSettingsService(settingsRepo);
            var config = new ConfigurationBaseProxy(svc.Object, "Global");

            string cultureCode = "en-US";
            config.SupportedType = CultureInfo.GetCultureInfo(cultureCode);
            Assert.AreEqual(cultureCode, config.SupportedType.Name);
        }
    }

    [TestFixture]
    public class GetSettingNamesTests
    {
        private class ConfigurationBaseNamesProxy : ConfigurationBase
        {
            public object Property1
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

            public CultureInfo Property2
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

            public ConfigurationBaseNamesProxy(ISettingsService svc, string prefix) :
                base(svc, prefix, null)
            {
            }
        }

        [Test]
        public void GetsAllFullyQualifiedSettingNames()
        {
            string @namespace = "System.Test";
            var config = new ConfigurationBaseNamesProxy(GetMockSettingsService().Object, @namespace);

            var expectedNames = new[]
            {
                $"{@namespace}.{nameof(ConfigurationBaseNamesProxy.Property1)}",
                $"{@namespace}.{nameof(ConfigurationBaseNamesProxy.Property2)}",
            };

            Assert.IsTrue(Enumerable.SequenceEqual(expectedNames.OrderBy(x => x), config.GetSettingNames().OrderBy(x => x)));
        }
    }

    [TestFixture]
    public class DescriptionTests
    {
        private class ConfigurationBaseDescriptionProxy : ConfigurationBase
        {
            public const string DESCRIBED_PROEPRTY_DESCRIPTION = "Description";

            public ConfigurationBaseDescriptionProxy(ISettingsService svc) :
                base(svc)
            {

            }

            [System.ComponentModel.Description(DESCRIBED_PROEPRTY_DESCRIPTION)] // there's a conflict between System.ComponentModel.Description and NUnitFramework.Description
            public int DescribedProperty
            {
                get
                {
                    return ReadValue<int>();
                }

                set
                {
                    SetValue(value);
                }
            }

            public double PropertyWithNoDescription
            {
                get
                {
                    return ReadValue<double>();
                }

                set
                {
                    SetValue(value);
                }
            }
        }

        [Test]
        public void GetsDescriptionForProperty()
        {
            var config = new ConfigurationBaseDescriptionProxy(GetMockSettingsService().Object);

            Assert.AreEqual(ConfigurationBaseDescriptionProxy.DESCRIBED_PROEPRTY_DESCRIPTION, config.GetDescriptionFor(nameof(config.DescribedProperty)));
        }

        [Test]
        public void ThrowsExceptionIfNullName()
        {
            var config = new ConfigurationBaseDescriptionProxy(GetMockSettingsService().Object);

            Assert.Throws<ArgumentNullException>(() =>
            {
                config.GetDescriptionFor(null);
            });
        }

        [Test]
        public void ReturnsEmptyStringIfNoDescription()
        {
            var config = new ConfigurationBaseDescriptionProxy(GetMockSettingsService().Object);

            Assert.AreEqual(string.Empty, config.GetDescriptionFor(nameof(config.PropertyWithNoDescription)));
        }
    }
}
