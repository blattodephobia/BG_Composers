using BGC.Core.Services;
using BGC.Web.Areas.Public.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BGC.WebAPI.Tests.PublicArea.Controllers
{
    [TestFixture]
    public class UploadTests
    {
        private ControllerContext GetMockControllerContext(ControllerBase ctrl)
        {
            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(x => x.ApplicationPath).Returns("/");
            request.SetupGet(x => x.Url).Returns(new Uri("http://localhost/a", UriKind.Absolute));
            request.SetupGet(x => x.ServerVariables).Returns(new NameValueCollection());

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.Setup(x => x.ApplyAppPathModifier("/post1")).Returns("http://localhost/post1");

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.Response).Returns(response.Object);

            return new ControllerContext(context.Object, new RouteData(), ctrl);
        }

        [Test]
        public void GeneratesCorrectUriForUploadedResource()
        {
            Mock<IMediaService> mockMediaService = new Mock<IMediaService>();
            Guid fakeId = new Guid(Enumerable.Range(0, 16).Select(i => (byte)i).ToArray());
            mockMediaService.Setup(s => s.AddMedia(It.IsAny<ContentType>(), It.IsAny<Stream>(), It.IsAny<string>())).Returns(fakeId);
            ResourcesController ctrl = new ResourcesController(mockMediaService.Object);
            ctrl.ControllerContext = GetMockControllerContext(ctrl);
            ctrl.Url = new UrlHelper(new RequestContext(ctrl.ControllerContext.HttpContext, new RouteData()), new RouteCollection());

            Mock<HttpPostedFileBase> mockFile = new Mock<HttpPostedFileBase>();
            mockFile.Setup(f => f.InputStream).Returns(new MemoryStream());
            mockFile.Setup(f => f.ContentType).Returns("image/jpeg");
            mockFile.Setup(f => f.FileName).Returns("image.jpeg");
            ContentResult result = ctrl.Upload(mockFile.Object) as ContentResult;
            Assert.AreEqual($"http://localhost/?resourceId={fakeId.ToString()}", result.Content);
        }
    }
}
