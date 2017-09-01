using BGC.Core;
using BGC.Core.Models;
using BGC.Web.Areas.Administration.Controllers;
using BGC.Web.Areas.Administration.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static TestUtils.MockUtilities;

namespace BGC.Web.Tests.AdministrationArea.Controllers.GlossaryControllerTests
{
    [TestFixture]
    public class EditTest
    {
        [Test]
        public void ReturnsViewForEditingIfNoId()
        {
            var ctrl = new GlossaryController(GetMockGlossaryService(new List<GlossaryEntry>()).Object);
            ctrl.ApplicationProfile = GetStandardAppProfile();
            ActionResult result = ctrl.Edit(null);

            Assert.IsTrue(result is ViewResult);
        }

        [Test]
        public void ReturnsViewModelForEditingIfNoId()
        {
            var ctrl = new GlossaryController(GetMockGlossaryService(new List<GlossaryEntry>()).Object);
            ctrl.ApplicationProfile = GetStandardAppProfile();
            ActionResult result = ctrl.Edit(null);

            var view = result as ViewResult;
            var viewModel = view.Model as GlossaryEntryViewModel;

            Assert.IsNotNull(viewModel);
            Assert.AreEqual(null, viewModel.Id);
        }

        [Test]
        public void ReturnsViewModelForEditingIfNoIdWithCorrectNumberOfLanguages()
        {
            var ctrl = new GlossaryController(GetMockGlossaryService(new List<GlossaryEntry>()).Object);
            var supportedLanguages = new[] { new CultureInfo("en-US"), new CultureInfo("de-DE") };
            ctrl.ApplicationProfile = new ApplicationProfile() { SupportedLanguages = supportedLanguages };

            ActionResult result = ctrl.Edit(null);

            var view = result as ViewResult;
            var viewModel = view.Model as GlossaryEntryViewModel;

            Assert.IsNotNull(viewModel);
            Assert.IsTrue(supportedLanguages.SequenceEqual(viewModel.Definitions.Select(def => def.GetLocaleCultureInfo())));
        }

        [Test]
        public void ReturnsViewForUpdateIfCorrectId()
        {
            Guid id = Guid.NewGuid();
            var existingEntry = new GlossaryEntry() { Id = id };
            var ctrl = new GlossaryController(GetMockGlossaryService(new List<GlossaryEntry>() { existingEntry }).Object);
            ctrl.ApplicationProfile = GetStandardAppProfile();

            ActionResult result = ctrl.Edit(id);
            Assert.IsTrue(result is ViewResult);
        }

        [Test]
        public void ReturnsViewModelForUpdateIfCorrectId()
        {
            Guid id = Guid.NewGuid();
            var definition = new GlossaryDefinition(new CultureInfo("en-US"), "Hey", "term");
            var existingEntry = new GlossaryEntry()
            {
                Id = id,
                Definitions = new List<GlossaryDefinition>() { definition }
            };
            var ctrl = new GlossaryController(GetMockGlossaryService(new List<GlossaryEntry>() { existingEntry }).Object);
            ctrl.ApplicationProfile = GetStandardAppProfile();

            ViewResult result = ctrl.Edit(id) as ViewResult;
            var viewModel = result.Model as GlossaryEntryViewModel;

            Assert.AreEqual(id, viewModel.Id);
            Assert.AreEqual(definition.Definition, viewModel.GetDefinitionsInLocale(definition.Language).First().Definition);
        }

        [Test]
        public void ReturnsNotFoundIfIdNotPresent()
        {
            var ctrl = new GlossaryController(GetMockGlossaryService(new List<GlossaryEntry>()).Object);
            ctrl.ApplicationProfile = GetStandardAppProfile();
            ActionResult result = ctrl.Edit(default(Guid));

            Assert.IsTrue(result is HttpNotFoundResult);
        }

        [Test]
        public void CallsServiceUponPost()
        {
            var id = new Guid("01234567-0004-0004-0004-0123456789AB");
            var backingStore = new List<GlossaryEntry>();
            Mock<IGlossaryService> mockSvc = GetMockGlossaryService(backingStore);
            var ctrl = new GlossaryController(mockSvc.Object);
            ctrl.ApplicationProfile = GetStandardAppProfile();

            var postData = new GlossaryEntryViewModel() { Id = id };
            ctrl.Edit_Post(postData);

            mockSvc.Verify(svc => svc.AddOrUpdate(It.Is<GlossaryEntry>(g => g.Id == id)));
            Assert.AreEqual(id, backingStore.Single().Id);
        }
    }

    [TestFixture]
    public class DeleteTests
    {
        [Test]
        public void SetsStatusCodeToNotFoundIfInvalidId()
        {
            var mockSvc = GetMockGlossaryService(new List<GlossaryEntry>());
            var ctrl = new GlossaryController(mockSvc.Object);
            ctrl.ApplicationProfile = GetStandardAppProfile();
            ctrl.ControllerContext = new ControllerContext();
            Mock<HttpResponseBase> response = GetMockResponseBase(MockBehavior.Loose);
            response.SetupAllProperties();
            ctrl.ControllerContext.HttpContext = GetMockHttpContextBase(response: response.Object).Object;

            Guid missingId = new Guid("01234567-0123-0123-0123-0123456789AB");
            ctrl.Delete(missingId);
            Assert.AreEqual((int)HttpStatusCode.NotFound, ctrl.Response.StatusCode);
        }

        [Test]
        public void SetsStatusCodeToSuccessIfValidId()
        {
            Guid entryId = new Guid("01234567-0123-0123-0123-0123456789AB");
            var mockSvc = GetMockGlossaryService(new List<GlossaryEntry>()
            {
                new GlossaryEntry() { Id = entryId }
            });
            var ctrl = new GlossaryController(mockSvc.Object);
            ctrl.ApplicationProfile = GetStandardAppProfile();
            ctrl.ControllerContext = new ControllerContext();
            Mock<HttpResponseBase> response = GetMockResponseBase(MockBehavior.Loose);
            response.SetupAllProperties();
            response.Object.StatusCode = (int)HttpStatusCode.OK;
            ctrl.ControllerContext.HttpContext = GetMockHttpContextBase(response: response.Object).Object;

            ctrl.Delete(entryId);
            Assert.AreEqual((int)HttpStatusCode.OK, ctrl.Response.StatusCode);
        }
    }
}
