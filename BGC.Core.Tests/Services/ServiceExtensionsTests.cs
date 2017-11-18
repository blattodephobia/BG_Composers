using BGC.Core.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;
using static TestUtils.MockUtilities;

namespace BGC.Core.Tests.Services.ServiceExtensionsTests
{
    [TestFixture]
    public class ReadSettingTests : TestFixtureBase
    {
        [Test]
        public void NoExceptionNullSetting()
        {
            Mock<ISettingsService> mockSvc = GetMockSettingsService(new List<Setting>());
            mockSvc.Setup(s => s.ReadSetting(It.IsAny<string>())).Returns(value: null);
            
            var setting = ServiceExtensions.ReadSetting<DateTimeSetting>(mockSvc.Object, "test");
            Assert.IsNull(setting);
        }

        [Test]
        public void ThrowsExceptionIfWrongType()
        {
            Setting sampleSetting = new DateTimeSetting("test");
            Mock<ISettingsService> mockSvc = GetMockSettingsService(new List<Setting>() { sampleSetting });
            
            Assert.Throws<InvalidCastException>(() =>
            {
                var setting = ServiceExtensions.ReadSetting<CultureInfoSetting>(mockSvc.Object, sampleSetting.Name);
            });
        }

        [Test]
        public void CastsCorrectly()
        {
            Setting sampleSetting = new DateTimeSetting("test");
            Mock<ISettingsService> mockSvc = GetMockSettingsService(new List<Setting>() { sampleSetting });


            Assert.IsNotNull(ServiceExtensions.ReadSetting<DateTimeSetting>(mockSvc.Object, sampleSetting.Name));
        }
    }
}
