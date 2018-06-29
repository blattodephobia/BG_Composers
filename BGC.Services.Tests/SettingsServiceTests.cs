using BGC.Core;
using BGC.Core.Services;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;
using static TestUtils.MockUtilities;
using BGC.Data;

namespace BGC.Services.Tests.SettingsServiceTests
{
    public class FindSettingTests : TestFixtureBase
    {
        [Test]
        public void GetsHighestPrioritySettingFirst()
        {
            Mock<IRepository<Setting>> mockSettingsRepo = new Mock<IRepository<Setting>>();
            mockSettingsRepo
                .Setup(x => x.All())
                .Returns(new List<Setting>()
                {
                    new DateTimeSetting("TestSetting") { Priority = SettingPriority.Application }
                }.AsQueryable());

            BgcUser mockUser = new BgcUser("Alice");
            mockUser.UserSettings = new List<Setting>()
                {
                    new DateTimeSetting("TestSetting") { Priority = SettingPriority.User }
                };

            SettingsService service = new SettingsService(mockSettingsRepo.Object, mockUser);
            Setting setting = service.ReadSetting("TestSetting");
            Assert.AreEqual(mockUser.UserSettings.First(), setting);
        }

        [Test]
        public void CanHandleNullUser()
        {
            DateTimeSetting result = new DateTimeSetting("TestSetting") { Priority = SettingPriority.Application };
            Mock<IRepository<Setting>> mockSettingsRepo = new Mock<IRepository<Setting>>();
            mockSettingsRepo
                .Setup(x => x.All())
                .Returns(new List<Setting>() { result, new DateTimeSetting("OtherSetting") }.AsQueryable());

            SettingsService service = new SettingsService(mockSettingsRepo.Object);
            Setting setting = service.ReadSetting("TestSetting");
            Assert.AreEqual(result, setting);
        }

        [Test]
        public void FindSettingWithMultipleUsers()
        {
            DateTimeSetting appSetting = new DateTimeSetting("TestSetting") { Priority = SettingPriority.Application };
            DateTimeSetting userSetting1 = new DateTimeSetting("TestSetting") { Priority = SettingPriority.User };
            DateTimeSetting userSetting2 = new DateTimeSetting("TestSetting") { Priority = SettingPriority.User };

            Mock<IRepository<Setting>> mockSettingsRepo = new Mock<IRepository<Setting>>();
            mockSettingsRepo
                .Setup(x => x.All())
                .Returns(new List<Setting>() { appSetting, userSetting1, userSetting2 }.AsQueryable());

            BgcUser user = new BgcUser("Alice") { UserSettings = new List<Setting>() { userSetting1 } };

            SettingsService service = new SettingsService(mockSettingsRepo.Object, user);
            DateTimeSetting setting = service.ReadSetting<DateTimeSetting>("TestSetting");
            Assert.AreSame(userSetting1, setting);
        }

        [Test]
        public void DoesntReturnOtherUsersSettings()
        {
            string setting1Name = "TestSetting1";
            string setting2Name = "TestSetting2";
            DateTimeSetting appSetting = new DateTimeSetting(setting2Name) { Priority = SettingPriority.Application };
            DateTimeSetting userSetting1 = new DateTimeSetting(setting1Name) { Priority = SettingPriority.User };
            DateTimeSetting userSetting2 = new DateTimeSetting(setting2Name) { Priority = SettingPriority.User };

            Mock<IRepository<Setting>> mockSettingsRepo = new Mock<IRepository<Setting>>();
            mockSettingsRepo
                .Setup(x => x.All())
                .Returns(new List<Setting>() { appSetting, userSetting2, userSetting1 }.AsQueryable());

            BgcUser user = new BgcUser("Alice") { UserSettings = new List<Setting>() { userSetting1 } };

            SettingsService service = new SettingsService(mockSettingsRepo.Object, user);

            Setting expected = appSetting;
            Setting actual = service.ReadSetting(setting2Name);
            Assert.AreSame(expected, actual, "The service returns the wrong user's setting.");
        }
    }

    public class WriteSettingsTest : TestFixtureBase
    {
        [Test]
        public void UpdatesExistingSetting()
        {
            var existingSetting = new Setting("TestSetting");
            List<Setting> backingStore = new List<Setting>() { existingSetting };
            IRepository<Setting> settings = GetMockRepository(backingStore).Object;

            var svc = new SettingsService(settings);
            string testValue = "TestHarness";
            svc.WriteSetting(new Setting(existingSetting.Name) { StringValue = testValue });

            Assert.AreEqual(testValue, existingSetting.StringValue);
        }

        [Test]
        public void UpdatesExistingSetting_DifferentType()
        {
            var existingSetting = new DateTimeSetting("TestSetting");
            List<Setting> backingStore = new List<Setting>() { existingSetting };
            IRepository<Setting> settings = GetMockRepository(backingStore).Object;

            var svc = new SettingsService(settings);
            var testValue = new DateTime(2000, 1, 1);
            svc.WriteSetting(new DateTimeSetting(existingSetting.Name) { Date = testValue });

            Assert.AreEqual(testValue, existingSetting.Date);
        }

        [Test]
        public void DoesntUpdateOtherUsersSettings()
        {
            var existingSetting1 = new DateTimeSetting("TestSetting");
            var existingSetting2 = new DateTimeSetting(existingSetting1.Name) { OwnerStamp = "TheTest" };
            List<Setting> backingStore = new List<Setting>() { existingSetting2, existingSetting1 };
            IRepository<Setting> settings = GetMockRepository(backingStore).Object;

            var svc = new SettingsService(settings);
            var testValue = new DateTime(2000, 1, 1);
            svc.WriteSetting(new DateTimeSetting(existingSetting1.Name) { Date = testValue });

            Assert.AreEqual(testValue, existingSetting1.Date);
            Assert.AreNotEqual(testValue, existingSetting2.Date);
        }

        [Test]
        public void UpdatesSettingsWithSameBaseType()
        {
            var existingSetting = new DateTimeSetting("TestSetting");
            List<Setting> backingStore = new List<Setting>() { existingSetting };
            IRepository<Setting> settings = GetMockRepository(backingStore).Object;

            var svc = new SettingsService(settings);
            var testValue = new DateTime(2000, 1, 1);
            var settingWrapper = new Mock<DateTimeSetting>(existingSetting.Name) { CallBase = true };
            settingWrapper.Object.Date = testValue;

            svc.WriteSetting(settingWrapper.Object);

            Assert.AreEqual(testValue, existingSetting.Date);
        }

        [Test]
        public void ThrowsExceptionIfWrongSettingType()
        {
            var existingSetting = new DateTimeSetting("TestSetting") { Date = new DateTime(2000, 1, 1) };
            List<Setting> backingStore = new List<Setting>() { existingSetting };
            IRepository<Setting> settings = GetMockRepository(backingStore).Object;

            var svc = new SettingsService(settings);

            Assert.Throws<SettingException>(() => svc.WriteSetting(new Setting(existingSetting.Name) { StringValue = existingSetting.StringValue }));
        }
    }
}
