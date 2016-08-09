using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models.Media
{
    [TestFixture]
    public class MimeTypeInternalTests
    {
        [Test]
        public void IsSetFromInternalPropertyCorrectly()
        {
            MediaTypeInfo mt = new MediaTypeInfo("file", new ContentType("image/jpeg"));
            Assert.AreEqual(MediaTypeNames.Image.Jpeg, mt.MimeType.MediaType);
        }

        [Test]
        public void IsSetFromPublicPropertyCorrectly()
        {
            MediaTypeInfo mt = new MediaTypeInfo("file", new ContentType("image/jpeg"));
            Assert.AreEqual(MediaTypeNames.Image.Jpeg, mt.MimeTypeInternal);
        }
    }
}
