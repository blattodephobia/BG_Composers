using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models.Media
{
    [TestClass]
    public class MediaTypeInfoTests
    {
        [TestClass]
        public class MimeTypeInternalTests
        {
            [TestMethod]
            public void IsSetFromInternalPropertyCorrectly()
            {
                MediaTypeInfo mt = new MediaTypeInfo("file", new ContentType("image/jpeg"));
                Assert.AreEqual(MediaTypeNames.Image.Jpeg, mt.MimeType.MediaType);
            }

            [TestMethod]
            public void IsSetFromPublicPropertyCorrectly()
            {
                MediaTypeInfo mt = new MediaTypeInfo("file", new ContentType("image/jpeg"));
                Assert.AreEqual(MediaTypeNames.Image.Jpeg, mt.MimeTypeInternal);
            }
        }
    }
}
