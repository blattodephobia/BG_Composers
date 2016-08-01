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
    public class MediaTypeInfo : BgcEntity<long>
    {
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

        [NotMapped]
        public Stream Content { get; set; }
        
        [Unicode, Required, MaxLength(255)]
        public string OriginalFileName { get; set; }

        public bool IsTemporary => AsociatedArticles.Any();

        // This constructor is added for Entity Framework's sake
        protected MediaTypeInfo()
        {
        }

        public MediaTypeInfo(string fileName, string contentType) :
            this(fileName, new ContentType(contentType))
        {
        }

        public MediaTypeInfo(string fileName, ContentType contentType)
        {
            this.OriginalFileName = fileName;
            this.MimeType = contentType;
        }
    }
}
