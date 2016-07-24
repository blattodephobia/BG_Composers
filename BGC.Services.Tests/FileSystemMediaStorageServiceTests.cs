using BGC.Core;
using BGC.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services.Tests
{
    [TestClass]
    public class FileSystemMediaStorageServiceTests
    {
        [TestClass]
        public class GetMediaTests
        {
            [TestMethod]
            public void ReturnsNullOnNoMediaFound()
            {
                Mock<IRepository<ComposerArticle>> mockArticleRepo = new Mock<IRepository<ComposerArticle>>();
                mockArticleRepo.SetupAllProperties();
                Mock<IRepository<MediaTypeInfo>> mockMediaRepo = new Mock<IRepository<MediaTypeInfo>>();
                mockMediaRepo.Setup(a => a.All()).Returns(new[] { new MediaTypeInfo() { StorageId = new Guid(1, 2, 3, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }) } }.AsQueryable());
                
                FileSystemMediaService service = new FileSystemMediaService(
                    new DirectoryInfo(Environment.CurrentDirectory),
                    mockMediaRepo.Object,
                    mockArticleRepo.Object);
                Guid missingId = new Guid(11, 22, 33, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });

                Assert.IsNull(service.GetMedia(missingId));
            }
        }
    }
}
