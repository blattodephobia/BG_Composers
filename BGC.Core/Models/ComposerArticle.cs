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
        private CultureInfo _language;
        private ICollection<MediaTypeInfo> _media;
        
        [MaxLength(5)]
        internal protected string LanguageInternal
        {
            get
            {
                return _language.Name;
            }

            set
            {
                _language = CultureInfo.GetCultureInfo(value.ValueNotNull());
            }
        }

        [NotMapped]
        public CultureInfo Language
        {
            get
            {
                return _language;
            }

            set
            {
                _language = value.ValueNotNull();
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
                return _media ?? (_media = new HashSet<MediaTypeInfo>());
            }

            set
            {
                _media = value;
            }
        }

        public bool IsArchived { get; set; }

        public DateTime CreatedUtc { get; set; }

        public ComposerArticle()
        {
            CreatedUtc = DateTime.UtcNow;
        }

        public ComposerArticle(Composer composer, CultureInfo culture):
            this()
        {
            Composer = composer;
            LocalizedName = composer.GetName(culture);
            Language = culture;
        }
	}
}
