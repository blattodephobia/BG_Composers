using BGC.Core;
using BGC.Services;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BGC.Data;

namespace BGC.Services.Tests.FileSystemMediaStorageServiceTests
{
    [TestFixture]
    public class GetMediaTests
    {
        [Test]
        public void ReturnsNullOnNoMediaFound()
        {
            Mock<IRepository<ComposerArticle>> mockArticleRepo = new Mock<IRepository<ComposerArticle>>();
            mockArticleRepo.SetupAllProperties();
            Mock<IMediaTypeInfoRepository> mockMediaRepo = new Mock<IMediaTypeInfoRepository>();
            var media = new MediaTypeInfo("file", "image/jpeg") { StorageId = new Guid(1, 2, 3, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }) };
            Dictionary <Guid, MediaTypeInfo> backingStore = new Dictionary<Guid, MediaTypeInfo>()
            {
                { media.StorageId, media }
            };
            mockMediaRepo.Setup(a => a.Find(It.IsAny<Guid>())).Returns((Guid g) => backingStore[g]);

            FileSystemMediaService service = new FileSystemMediaService(
                new DirectoryInfo(Environment.CurrentDirectory),
                mockMediaRepo.Object);
            Guid missingId = new Guid(11, 22, 33, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });

            Assert.IsNull(service.GetMedia(missingId));
        }
    }
}
