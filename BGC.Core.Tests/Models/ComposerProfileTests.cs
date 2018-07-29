using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Core.Tests.Models.ComposerProfileTests
{
    public class ProfilePictureTests : TestFixtureBase
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
    
    public class AddImageTests : TestFixtureBase
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
            MediaTypeInfo image = new MediaTypeInfo("image/*") { Id = new Guid(1, 0, 3, new byte[8]) };
            Assert.Throws<InvalidOperationException>(() => profile.UpdateMedia(new[] { txt, image, null }));
        }
    }

    public class ImagesTests : TestFixtureBase
    {
        [Test]
        public void IncludesImagesOnly()
        {
            ComposerProfile profile = new ComposerProfile();
            Guid[] imageIds = new Guid[]
            {
                new Guid(1, 1, 2, new byte[8]),
                new Guid(1, 1, 3, new byte[8]),
            };
            profile.Media = new List<MediaTypeInfo>()
            {
                new MediaTypeInfo("image/jpeg") { StorageId = imageIds[0] },
                new MediaTypeInfo("image/png") { StorageId = imageIds[1] },
                new MediaTypeInfo("text/plain")
            };

            IEnumerable<Guid> actualImageIds = profile.Images().Select(i => i.StorageId).OrderBy(x => x);
            Assert.IsTrue(imageIds.OrderBy(x => x).SequenceEqual(actualImageIds));
        }

        [Test]
        public void IncludesProfilePictureIfNotPresentInMedia()
        {
            ComposerProfile profile = new ComposerProfile();
            Guid[] imageIds = new Guid[]
            {
                new Guid(1, 1, 2, new byte[8]),
                new Guid(1, 1, 3, new byte[8]),
            };
            profile.Media = new List<MediaTypeInfo>()
            {
                new MediaTypeInfo("image/jpeg") { StorageId = imageIds[0] },
                new MediaTypeInfo("text/plain")
            };
            profile.ProfilePicture = new MediaTypeInfo("image/jpeg") { StorageId = imageIds[1] };

            IEnumerable<Guid> actualImageIds = profile.Images().Select(i => i.StorageId).OrderBy(x => x);
            Assert.IsTrue(imageIds.OrderBy(x => x).SequenceEqual(actualImageIds));
        }

        [Test]
        public void DoesntDuplicateProfilePicture()
        {
            ComposerProfile profile = new ComposerProfile();
            Guid[] imageIds = new Guid[]
            {
                new Guid(1, 1, 2, new byte[8]),
                new Guid(1, 1, 3, new byte[8]),
            };
            profile.Media = new List<MediaTypeInfo>()
            {
                new MediaTypeInfo("image/jpeg") { StorageId = imageIds[0] },
                new MediaTypeInfo("image/png") { StorageId = imageIds[1] },
                new MediaTypeInfo("text/plain")
            };
            profile.ProfilePicture = profile.Media.First();

            IEnumerable<Guid> actualImageIds = profile.Images().Select(i => i.StorageId).OrderBy(x => x);
            Assert.IsTrue(imageIds.OrderBy(x => x).SequenceEqual(actualImageIds));
        }
    }
}
