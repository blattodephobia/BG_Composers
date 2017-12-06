using BGC.Core;
using BGC.Core.Services;
using BGC.Web.Areas.Administration.Controllers;
using BGC.Web.Areas.Administration.Models;
using BGC.Web.Areas.Administration.ViewModels.Permissions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestUtils;
using static TestUtils.MockUtilities;

namespace BGC.Web.Tests.AdministrationArea.Controllers.SettingsControllerTests
{
    public class ApplicationSettingsActionTests : TestFixtureBase
    {
        [Test]
        public void InitializesViewModelCorrectly_SettingNames()
        {
            Mock<ISettingsService> svc = GetMockSettingsService();
            SettingsController ctrl = new SettingsController(svc.Object);

            var vm = (ctrl.ApplicationSettings() as ViewResult).Model as ApplicationSettingsWritePermissionViewModel;
            IEnumerable<string> expectedPropertiesNames = typeof(ApplicationConfiguration).GetProperties().Select(p => p.Name).OrderBy(x => x);
            IEnumerable<string> actualPropertiesNames = vm.Settings.Select(s => s.Name).OrderBy(x => x);

            Assert.IsTrue(expectedPropertiesNames.SequenceEqual(actualPropertiesNames));
        }

        [Test]
        public void InitializesViewModelCorrectly_SettingTypes()
        {
            Mock<ISettingsService> svc = GetMockSettingsService();
            SettingsController ctrl = new SettingsController(svc.Object);

            var vm = (ctrl.ApplicationSettings() as ViewResult).Model as ApplicationSettingsWritePermissionViewModel;
            IEnumerable<Type> expectedPropertiesTypes = typeof(ApplicationConfiguration).GetProperties().Select(p => p.PropertyType).OrderBy(x => x);
            IEnumerable<Type> actualPropertiesTypes = vm.Settings.Select(s => s.Type).OrderBy(x => x);

            Assert.IsTrue(expectedPropertiesTypes.SequenceEqual(actualPropertiesTypes));
        }

        [Test]
        public void SetsSettingsFromViewModelCorrectly()
        {
            List<Setting> repo = new List<Setting>();
            Mock<ISettingsService> svc = GetMockSettingsService(repo);
            SettingsController ctrl = new SettingsController(svc.Object);
            
            var vm = (ctrl.ApplicationSettings() as ViewResult).Model as ApplicationSettingsWritePermissionViewModel;
            foreach (SettingWebModel webModel in vm.Settings)
            {
                if (webModel.Type.GetConstructor(Type.EmptyTypes) != null)
                {
                    webModel.Value = Activator.CreateInstance(webModel.Type).ToString();
                }
                else
                {
                    webModel.Value = null;
                }
            }
            ctrl.ApplicationSettings_Post(vm);

            IEnumerable<string> expectedSettingNames = repo.Select(s => s.Name.Substring(s.Name.LastIndexOf(".") + 1)).OrderBy(x => x);
            IEnumerable<string> actualSettingNames = vm.Settings.Select(s => s.Name).OrderBy(x => x);

            Assert.IsTrue(expectedSettingNames.SequenceEqual(actualSettingNames));
        }
    }
}
