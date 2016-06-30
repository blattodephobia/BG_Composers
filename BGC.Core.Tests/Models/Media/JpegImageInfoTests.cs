using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models.Media
{
    [TestClass]
    public class JpegImageInfoTests
    {
        [TestClass]
        public class JFifHeaderTests
        {
            [TestMethod]
            public void ReturnsCorrectHeaderWithValidImage()
            {
                JpegImageInfo img = new JpegImageInfo(File.OpenRead(@"Models\Media\jpgimagetest.jpg"));
                img.ValidateHeader();
            }
        }
    }
}
