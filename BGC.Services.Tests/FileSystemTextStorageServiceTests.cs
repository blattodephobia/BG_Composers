using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TestUtils.PlatformUtilities;

namespace BGC.Services.Tests.FileSystemTextStorageServiceTests
{
    class FSArticleStorageServiceProxy : FileSystemArticleContentService
    {
        public FSArticleStorageServiceProxy(string dirPath) :
            base(new DirectoryInfo(dirPath))
        {
        }

        public new FileInfo GuidToFileName(Guid guid) => base.GuidToFileName(guid);

        public new Guid FileNameToGuid(FileInfo file) => base.FileNameToGuid(file);
    }

    [TestFixture]
    public class GuidToFileNameTests
    {
        [Test]
        public void GeneratesValidFileName()
        {
            FSArticleStorageServiceProxy svc = new FSArticleStorageServiceProxy(@".");
            Guid guid = Guid.Parse("789ABCDE-00FB-ADCC-1111-0123456789AB");
            Assert.AreEqual("789ABCDE-00FB-ADCC-1111-0123456789AB", ShortFileName(svc.GuidToFileName(guid).Name));
        }
    }

    [TestFixture]
    public class FileNameToGuidTests
    {
        [Test]
        public void ParsesNameCorrectly()
        {
            Guid guid = Guid.Parse("789ABCDE-00FB-ADCC-1111-0123456789AB");
            FSArticleStorageServiceProxy svc = new FSArticleStorageServiceProxy(@"C:\");
            Assert.AreEqual(guid, svc.FileNameToGuid(new FileInfo("789ABCDE-00FB-ADCC-1111-0123456789AB")));
        }
    }
}
