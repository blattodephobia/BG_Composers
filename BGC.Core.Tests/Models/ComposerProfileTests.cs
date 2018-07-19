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

                profile.ProfilePicture = pic;
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
                Guid id1 = new Guid(1, 0, 0, new byte[8]);
                Guid id2 = new Guid(2, 0, 0, new byte[8]);
                profile.Media = new List<MediaTypeInfo>()
                {
                    new MediaTypeInfo("image/*") { Id = id1 },
                    new MediaTypeInfo("image/*") { Id = id2 }
                };

                MediaTypeInfo existing = new MediaTypeInfo("image/*") { Id = id1 };
                MediaTypeInfo @new = new MediaTypeInfo("image/*") { Id = new Guid(3, 0, 0, new byte[8]) };

                profile.UpdateMedia(new[] { existing, @new });

                Assert.AreSame(existing, profile.Media.First(m => m.Id == existing.Id));
                Assert.IsTrue(profile.Media.Contains(@new));
                Assert.AreEqual(3, profile.Media.Count);
            }

            [Test]
            public void ThrowsExceptionIfNotValidImage()
            {
                ComposerProfile profile = new ComposerProfile();

                MediaTypeInfo txt = new MediaTypeInfo("text/plain") { Id = new Guid(1, 0, 1, new byte[8]) };
                MediaTypeInfo image = new MediaTypeInfo("image/*") { Id =  new Guid(1, 0, 3, new byte[8]) };
                Assert.Throws<InvalidOperationException>(() => profile.UpdateMedia(new[] { txt, image, null }));
            }
        }
    }
}
