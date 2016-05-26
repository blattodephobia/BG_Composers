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
                Mock<IRepository<ApplicationSetting>> mockAppSettingsRepo = new Mock<IRepository<ApplicationSetting>>();
                mockAppSettingsRepo
                    .Setup(x => x.All())
                    .Returns(new List<ApplicationSetting>()
                    {
                        new ApplicationSetting() { Name = "TestSetting" }
                    }.AsQueryable());

                Mock<IRepository<UserSetting>> mockUserSettingsRepo = new Mock<IRepository<UserSetting>>();
                mockUserSettingsRepo
                    .Setup(x => x.All())
                    .Returns(new List<UserSetting>()
                    {
                        new UserSetting() { Name = "TestSetting" }
                    }.AsQueryable());

                SettingsService service = new SettingsService(mockAppSettingsRepo.Object, mockUserSettingsRepo.Object);
                Setting setting = service.FindSetting("TestSetting");
                Assert.IsTrue(setting is UserSetting);
            }
        }
    }
}
