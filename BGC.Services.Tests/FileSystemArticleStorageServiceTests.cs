using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services.Tests
{
    [TestClass]
    public class FileSystemArticleStorageServiceTests
    {
        class FSArticleStorageServiceProxy : FileSystemDataStorageService
        {
            public FSArticleStorageServiceProxy() :
                base(new DirectoryInfo(@"C:\"))
            {
            }

            public new FileInfo GuidToFileName(Guid guid) => base.GuidToFileName(guid);

            public new Guid FileNameToGuid(FileInfo file) => base.FileNameToGuid(file);
        }

        [TestClass]
        public class GuidToFileNameTests
        {
            [TestMethod]
            public void GeneratesValidFileName()
            {
                FSArticleStorageServiceProxy svc = new FSArticleStorageServiceProxy();
                Guid guid = Guid.Parse("789ABCDE-00FB-ADCC-1111-0123456789AB");
                Assert.AreEqual("789ABCDE-00FB-ADCC-1111-0123456789AB", svc.GuidToFileName(guid).Name);
            }

            [TestMethod]
            public void GeneratesFileNameInCorrectDirectory()
            {
                FSArticleStorageServiceProxy svc = new FSArticleStorageServiceProxy();
                Guid guid = Guid.Parse("789ABCDE-00FB-ADCC-1111-0123456789AB");
                Assert.AreEqual("C:\\", svc.GuidToFileName(guid).Directory.Name);
            }
        }

        [TestClass]
        public class FileNameToGuidTests
        {
            [TestMethod]
            public void ParsesNameCorrectly()
            {
                Guid guid = Guid.Parse("789ABCDE-00FB-ADCC-1111-0123456789AB");
                FSArticleStorageServiceProxy svc = new FSArticleStorageServiceProxy();
                Assert.AreEqual(guid, svc.FileNameToGuid(new FileInfo("789ABCDE-00FB-ADCC-1111-0123456789AB")));
            }
        }
    }
}
