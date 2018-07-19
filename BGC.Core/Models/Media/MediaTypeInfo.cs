using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    [Table(nameof(MediaTypeInfo) + "s")] // Entity Framework maps this entity to "MediaTypeInfoes", which is incorrect
    public class MediaTypeInfo : BgcEntity<Guid>
    {
        public static MediaTypeInfo NewExternalMedia(string location, ContentType mimeType = null)
        {
            MediaTypeInfo result = new MediaTypeInfo();
            result.ExternalLocation = location;
            result.MimeType = mimeType ?? new ContentType(MediaTypeNames.Application.Octet);

            return result;
        }

        private ContentType mimeType;
        private ICollection<ComposerArticle> asociatedArticles;

        [Required]
        internal protected string MimeTypeInternal
        {
            get
            {
                return this.mimeType.ToString();
            }
            
            set
            {
                this.mimeType = new ContentType(value);
            }   
        }

        public override Guid Id { get => StorageId; set => StorageId = value; }

        [Index]
        public Guid StorageId { get; set; }

        [NotMapped]
        public ContentType MimeType
        {
            get
            {
                return this.mimeType;
            }

            set
            {
                this.mimeType = value;
            }
        }

        public virtual ICollection<ComposerArticle> AsociatedArticles
        {
            get
            {
                return this.asociatedArticles ?? (this.asociatedArticles = new HashSet<ComposerArticle>());
            }

            set
            {
                this.asociatedArticles = value;
            }
        }
        
        [Unicode, MaxLength(255)]
        public string OriginalFileName { get; set; }

        [MaxLength(512)]
        public string ExternalLocation { get; set; }

        // This constructor is added for Entity Framework's sake
        protected MediaTypeInfo()
        {
        }

        public MediaTypeInfo(string contentType) :
            this(null, contentType)
        {
        }

        public MediaTypeInfo(string fileName, string contentType) :
            this(fileName, new ContentType(contentType))
        {
        }

        public MediaTypeInfo(string fileName, ContentType contentType)
        {
            OriginalFileName = fileName;
            MimeType = contentType;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj as MediaTypeInfo)?.Id == Id;
        }
    }
}
