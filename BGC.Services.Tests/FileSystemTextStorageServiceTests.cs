using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services.Tests
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
            FSArticleStorageServiceProxy svc = new FSArticleStorageServiceProxy(@"C:\");
            Guid guid = Guid.Parse("789ABCDE-00FB-ADCC-1111-0123456789AB");
            Assert.AreEqual("789ABCDE-00FB-ADCC-1111-0123456789AB", svc.GuidToFileName(guid).Name);
        }

        [Test]
        public void GeneratesFileNameInCorrectDirectory()
        {
            FSArticleStorageServiceProxy svc = new FSArticleStorageServiceProxy(@"C:\Windows\");
            Guid guid = Guid.Parse("789ABCDE-00FB-ADCC-1111-0123456789AB");
            Assert.AreEqual("Windows", svc.GuidToFileName(guid).Directory.Name);
        }

        [Test]
        public void GeneratesFileNameInCorrectDirectory_NoBackslash()
        {
            FSArticleStorageServiceProxy svc = new FSArticleStorageServiceProxy(@"C:\Windows");
            Guid guid = Guid.Parse("789ABCDE-00FB-ADCC-1111-0123456789AB");
            Assert.AreEqual("Windows", svc.GuidToFileName(guid).Directory.Name);
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
