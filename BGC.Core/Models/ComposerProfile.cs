using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class ComposerProfile : BgcEntity<Guid>
    {
        private static bool IsImage(MediaTypeInfo media) => media?.MimeType.MediaType.StartsWith("image/") ?? false;

        private MediaTypeInfo _profilePicture;
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

        private ICollection<MediaTypeInfo> _selectedWorks;
        public virtual ICollection<MediaTypeInfo> SelectedWorks
        {
            get
            {
                return _selectedWorks ?? (_selectedWorks = new HashSet<MediaTypeInfo>());
            }

            set
            {
                _selectedWorks = value;
            }
        }
    }
}
