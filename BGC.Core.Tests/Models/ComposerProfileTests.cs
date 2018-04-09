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

        [TestFixture]
        public class AddImageTests
        {
            [Test]
            public void DoesntAddSameImageTwice()
            {
                ComposerProfile profile = new ComposerProfile();
                long id1 = 1;
                long id2 = 2;
                profile.Media = new List<MediaTypeInfo>()
                {
                    new MediaTypeInfo("image/*") { Id = id1 },
                    new MediaTypeInfo("image/*") { Id = id2 }
                };

                MediaTypeInfo existing = new MediaTypeInfo("image/*") { Id = id1 };
                MediaTypeInfo @new = new MediaTypeInfo("image/*") { Id = 3 };
                profile.UpdateMedia(new[] { existing, @new });

                Assert.AreSame(existing, profile.Media.First(m => m.Id == existing.Id));
                Assert.IsTrue(profile.Media.Contains(@new));
                Assert.AreEqual(3, profile.Media.Count);
            }

            [Test]
            public void ThrowsExceptionIfNotValidImage()
            {
                ComposerProfile profile = new ComposerProfile();

                MediaTypeInfo txt = new MediaTypeInfo("text/plain") { Id = 1 };
                MediaTypeInfo image = new MediaTypeInfo("image/*") { Id = 3 };
                Assert.Throws<InvalidOperationException>(() => profile.UpdateMedia(new[] { txt, image, null }));
            }
        }
    }
}
