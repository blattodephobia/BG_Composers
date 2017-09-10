using BGC.Core;
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
                        new DateTimeSetting() { Name = "TestSetting", Priority = SettingPriority.Application }
                }.AsQueryable());

            BgcUser mockUser = new BgcUser("Alice");
            mockUser.UserSettings = new List<Setting>()
                {
                    new DateTimeSetting() { Name = "TestSetting", Priority = SettingPriority.User }
                };

            SettingsService service = new SettingsService(mockSettingsRepo.Object, mockUser);
            Setting setting = service.ReadSetting("TestSetting");
            Assert.AreEqual(mockUser.UserSettings.First(), setting);
        }

        [Test]
        public void CanHandleNullUser()
        {
            DateTimeSetting result = new DateTimeSetting() { Name = "TestSetting", Priority = SettingPriority.Application };
            Mock<IRepository<Setting>> mockSettingsRepo = new Mock<IRepository<Setting>>();
            mockSettingsRepo
                .Setup(x => x.All())
                .Returns(new List<Setting>() { result, new DateTimeSetting() { Name = "OtherSetting" } }.AsQueryable());

            SettingsService service = new SettingsService(mockSettingsRepo.Object);
            Setting setting = service.ReadSetting("TestSetting");
            Assert.AreEqual(result, setting);
        }

        [Test]
        public void FindSettingWithMultipleUsers()
        {
            DateTimeSetting appSetting = new DateTimeSetting() { Name = "TestSetting", Priority = SettingPriority.Application };
            DateTimeSetting userSetting1 = new DateTimeSetting() { Name = "TestSetting", Priority = SettingPriority.User };
            DateTimeSetting userSetting2 = new DateTimeSetting() { Name = "TestSetting", Priority = SettingPriority.User };

            Mock<IRepository<Setting>> mockSettingsRepo = new Mock<IRepository<Setting>>();
            mockSettingsRepo
                .Setup(x => x.All())
                .Returns(new List<Setting>() { appSetting, userSetting1, userSetting2 }.AsQueryable());

            BgcUser user = new BgcUser("Alice") { UserSettings = new List<Setting>() { userSetting1 } };

            SettingsService service = new SettingsService(mockSettingsRepo.Object, user);
            DateTimeSetting setting = service.ReadSetting<DateTimeSetting>("TestSetting");
            Assert.AreSame(userSetting1, setting);
        }
    }
}
