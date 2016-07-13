using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class MediaTypeInfo : BgcEntity<long>
    {
        private ContentType mimeType;
        private ICollection<ComposerArticle> asociatedArticles;

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

        public Guid StorageId { get; set; }

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

        public ICollection<ComposerArticle> AsociatedArticles
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
    }
}
