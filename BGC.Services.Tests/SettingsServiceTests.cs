using BGC.Core;
using BGC.Core.Services;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services.Tests.SettingsServiceTests
{
    [TestFixture]
    public class FindSettingTests
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
}
