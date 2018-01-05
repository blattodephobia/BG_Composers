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
        private class ConfigurationBaseReadProxy : ConfigurationBase
        {
            public ConfigurationBaseReadProxy(ISettingsService svc, string settingsPrefix, SettingsFactory factory = null) :
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
            ConfigurationBaseReadProxy config = new ConfigurationBaseReadProxy(GetMockSettingsService().Object, null);
            string cultureCode = "de-DE";
            config.SupportedType = new CultureInfo(cultureCode);

            Assert.AreEqual(cultureCode, config.SupportedType.Name);
        }

        [Test]
        public void CreatesSettingInLazyManner()
        {
            List<Setting> settingsRepo = new List<Setting>();
            Mock<ISettingsService> svc = GetMockSettingsService(settingsRepo);
            ConfigurationBaseReadProxy config = new ConfigurationBaseReadProxy(svc.Object, null);

            config.SupportedType = new CultureInfo("bg-BG");

            Assert.IsNotNull(settingsRepo.FirstOrDefault(s => s.Name == nameof(config.SupportedType)));
        }

        [Test]
        public void AcceptsFullAndShortSettingNames()
        {
            string prefix = "Standard";
            var config = new ConfigurationBaseReadProxy(GetMockSettingsService().Object, prefix);

            var testCulture = new CultureInfo("cs-CZ");
            config.SupportedType = testCulture;

            Assert.AreEqual(testCulture, config.ReadValue<CultureInfo>(nameof(config.SupportedType)));
            Assert.AreEqual(testCulture, config.ReadValue<CultureInfo>($"{prefix}.{nameof(config.SupportedType)}"));
        }

        [Test]
        public void ThrowsExceptionIfNoStronglyTypedConversionAvailable()
        {
            List<Setting> settingsRepo = new List<Setting>();
            Mock<ISettingsService> faultySettingsService = GetMockSettingsService(settingsRepo);
            faultySettingsService.Setup(x => x.ReadSetting(It.IsAny<string>())).Returns(new Setting("wrong"));

            ConfigurationBaseReadProxy config = new ConfigurationBaseReadProxy(faultySettingsService.Object, null);

            Assert.Throws<SettingTypeMismatchException>(() =>
            {
                config.SupportedType = new CultureInfo("ja-JP");
            });
        }

        [Test]
        public void SetsAndReadsSettingsWithPrefix()
        {
            List<Setting> settingsRepo = new List<Setting>();
            Mock<ISettingsService> svc = GetMockSettingsService(settingsRepo);
            var config = new ConfigurationBaseReadProxy(svc.Object, "Global");

            string cultureCode = "en-US";
            config.SupportedType = CultureInfo.GetCultureInfo(cultureCode);
            Assert.AreEqual(cultureCode, config.SupportedType.Name);
        }
    }

    public class SetValueTests : TestFixtureBase
    {
        private class ConfigurationBaseWriteProxy : ConfigurationBase
        {
            public ConfigurationBaseWriteProxy(ISettingsService svc, string settingsPrefix, SettingsFactory factory = null) :
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
        public void AcceptsFullSettingNames()
        {
            string prefix = "Test.Default";
            var config = new ConfigurationBaseWriteProxy(GetMockSettingsService().Object, prefix);
            var testCulture = new CultureInfo("cs-CZ");

            config.SetValue(testCulture, $"{prefix}.{nameof(config.SupportedType)}");

            Assert.AreEqual(testCulture, config.SupportedType);
        }

        [Test]
        public void AcceptsShortSettingNames()
        {
            var config = new ConfigurationBaseWriteProxy(GetMockSettingsService().Object, null);
            var testCulture = new CultureInfo("cs-CZ");

            config.SetValue(testCulture, nameof(config.SupportedType));

            Assert.AreEqual(testCulture, config.SupportedType);
        }
    }
    
    public class AllSettingsTests : TestFixtureBase
    {
        private class ConfigurationBaseNamesProxy : ConfigurationBase
        {
            public string Property1
            {
                get
                {
                    return ReadValue<string>();
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

            Assert.IsTrue(Enumerable.SequenceEqual(expectedNames.OrderBy(x => x), config.AllSettings().Select(s => s.Name).OrderBy(x => x)));
        }

        [Test]
        public void ThrowsExceptionIfMismatchInSettingAndPropertyType()
        {
            List<Setting> repo = new List<Setting>()
            {
                new DateTimeSetting(nameof(ConfigurationBaseNamesProxy.Property2)) // Property2 is of type CultureInfo
            };

            var config = new ConfigurationBaseNamesProxy(GetMockSettingsService(repo).Object, null);

            Assert.Throws<SettingTypeMismatchException>(() =>
            {
                config.AllSettings();
            });
        }

        [Test]
        public void NoExceptionIfSettingAndPropertyTypesMatch()
        {
            List<Setting> repo = new List<Setting>()
            {
                new Setting(nameof(ConfigurationBaseNamesProxy.Property1))
            };

            var config = new ConfigurationBaseNamesProxy(GetMockSettingsService(repo).Object, null);
            config.AllSettings();
        }
    }
    
    public class DescriptionTests : TestFixtureBase
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
