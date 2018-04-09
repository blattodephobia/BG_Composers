using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class ComposerProfile : BgcEntity<Guid>
    {
        private static bool IsImage(MediaTypeInfo media) => media?.MimeType.MediaType.StartsWith("image/") ?? false;

        private MediaTypeInfo _profilePicture;
        [NotMapped]
        public virtual MediaTypeInfo ProfilePicture
        {
            get
            {
                return _profilePicture;
            }

            set
            {
                Shield.Assert(value, IsImage(value), (x) => new InvalidOperationException($"The new value for {nameof(ProfilePicture)} doesn't refer to a valid image.")).ThrowOnError();

                _profilePicture = value;
            }
        }

        public IEnumerable<MediaTypeInfo> Images() => Media.Where(m => IsImage(m));

        private ICollection<MediaTypeInfo> _media;
        [NotMapped]
        public virtual ICollection<MediaTypeInfo> Media
        {
            get
            {
                return _media ?? (_media = new HashSet<MediaTypeInfo>());
            }

            set
            {
                _media = value;
            }
        }

        /// <summary>
        /// Adds images from the <paramref name="media"/> collection or updates existing ones.
        /// </summary>
        /// <param name="media"></param>
        public void UpdateMedia(IEnumerable<MediaTypeInfo> media)
        {
            List<MediaTypeInfo> mediaToUpdate = new List<MediaTypeInfo>();

            foreach (MediaTypeInfo m in media)
            {
                Shield.Assert(
                    value: m,
                    condition: m != null,
                    exceptionProvider: (x) => new InvalidOperationException($"Cannot add or update a null value.")).ThrowOnError();

                if (Media.Contains(m))
                {
                    mediaToUpdate.Add(m);
                }
                else
                {
                    Media.Add(m);
                }
            }

            foreach (MediaTypeInfo m in mediaToUpdate)
            {
                Media.Remove(m);
                Media.Add(m);
            }
        }
    }
}
