using BGC.Core;
using BGC.Web.Areas.Public.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static TestUtils.MockUtilities;

namespace BGC.Web.Tests.AdministrationArea.Controllers
{
    [TestFixture]
    public class GetTests
    {
        [Test]
        public void ReturnsNotFoundIfNoMedia()
        {
            var controller = new ResourcesController(GetMockMediaService().Object);

            Assert.IsTrue(controller.Get(new Guid()) is HttpNotFoundResult);
        }

        [Test]
        public void ReturnsFileResultIfMediaFound()
        {
            Guid id = new Guid();
            List<MediaTypeInfo> metadata = new List<MediaTypeInfo>() { new MediaTypeInfo("C:\file.txt", "text/plain") { StorageId = id } };
            var controller = new ResourcesController(GetMockMediaService(metadata, m => Stream.Null).Object);

            Assert.IsTrue(controller.Get(new Guid()) is FileResult);
        }
    }
}
