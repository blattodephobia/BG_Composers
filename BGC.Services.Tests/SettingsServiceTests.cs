using BGC.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services.Tests
{
    [TestClass]
    public class SettingsServiceTests
    {
        [TestClass]
        public class FindSettingTests
        {
            [TestMethod]
            public void GetsHighestPrioritySettingFirst()
            {
                Mock<IRepository<Setting>> mockSettingsRepo = new Mock<IRepository<Setting>>();
                mockSettingsRepo
                    .Setup(x => x.All())
                    .Returns(new List<Setting>()
                    {
                        new DateTimeSetting() { Name = "TestSetting", Priority = SettingPriority.Application }
                    }.AsQueryable());

                BgcUser mockUser = new BgcUser();
                mockUser.UserSettings = new List<Setting>()
                {
                    new DateTimeSetting() { Name = "TestSetting", Priority = SettingPriority.User }
                };

                SettingsService service = new SettingsService(mockSettingsRepo.Object, mockUser);
                Setting setting = service.ReadSetting("TestSetting");
                Assert.AreEqual(mockUser.UserSettings.First(), setting);
            }

            [TestMethod]
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
        }
    }
}
