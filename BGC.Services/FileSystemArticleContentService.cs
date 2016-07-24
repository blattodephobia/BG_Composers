using BGC.Core.Services;
using CodeShield;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services
{
    internal class FileSystemArticleContentService : FileSystemStorageService<string>, IArticleContentService
    {
        public override string GetEntry(Guid id)
        {
            using (StreamReader read = GuidToFileName(id).OpenText())
            {
                return read.ReadToEnd();
            }
        }

        public override Guid StoreEntry(string text)
        {
            Guid newId = Guid.NewGuid();
            using (StreamWriter write = new StreamWriter(GuidToFileName(newId).OpenWrite()))
            {
                write.Write(text);
                return newId;
            }
        }

        public override void UpdateEntry(Guid id, string text)
        {
            using (StreamWriter write = new StreamWriter(GuidToFileName(id).OpenWrite()))
            {
                write.Write(text);
            }
        }

        public FileSystemArticleContentService([Dependency(ServiceLayerDependencyRegistration.DefaultDataStorageDirectoryKey)] DirectoryInfo storageDir) :
            base(storageDir)
        {
        }
    }
}
