using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models
{
    [TestFixture]
    public class ComposerProfileTests
    {
        [TestFixture]
        public class ProfilePictureTests
        {
            [Test]
            public void AcceptsImages()
            {
                var profile = new ComposerProfile();
                var pic = new MediaTypeInfo("profile.jpg", MediaTypeNames.Image.Jpeg);
            }

            [Test]
            public void ThrowsExceptionIfNotImage()
            {
                var profile = new ComposerProfile();
                Assert.Throws<InvalidOperationException>(() =>
                {
                    profile.ProfilePicture = new MediaTypeInfo("any.txt", MediaTypeNames.Text.Plain);
                });
            }
        }
    }
}
