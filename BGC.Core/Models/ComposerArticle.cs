using BGC.Core.Services;
using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	public partial class ComposerArticle : BgcEntity<long>
	{
        private CultureInfo language;
        private ICollection<MediaTypeInfo> media;
        
        [MaxLength(5)]
        internal protected string LanguageInternal
        {
            get
            {
                return this.language.Name;
            }

            set
            {
                this.language = CultureInfo.GetCultureInfo(value.ValueNotNull());
            }
        }

        [NotMapped]
        public CultureInfo Language
        {
            get
            {
                return this.language;
            }

            set
            {
                this.language = value.ValueNotNull();
            }
        }

        [Index]
        /// <summary>
        /// Gets or sets the <see cref="Guid"/> that identifies the actual text of this article, as stored in an external service (such as <see cref="IDataStorageService"/>).
        /// </summary>
		public Guid StorageId { get; set; }

        [Required]
		public virtual ComposerName LocalizedName { get; set; }

        [Required]
        public virtual Composer Composer { get; set; }

        public virtual ICollection<MediaTypeInfo> Media
        {
            get
            {
                return this.media ?? (this.media = new HashSet<MediaTypeInfo>());
            }

            set
            {
                this.media = value;
            }
        }
	}
}
