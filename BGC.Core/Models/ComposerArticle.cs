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

        private ComposerName _localizedName;
        [Required]
		public virtual ComposerName LocalizedName
        {
            get
            {
                return _localizedName;
            }

            set
            {
                Shield.ArgumentNotNull(value).ThrowOnError();
                Shield.AssertOperation(value, value.Language.Equals(Language), $"The specified name's locale didn't match the locale of the article. Expected {Language.Name}, but was {value.Language.Name}.").ThrowOnError();
                Shield.AssertOperation(value, value.Composer == null || value.Composer.Equals(Composer), $"The name {value.FullName} belongs to a composer other than this object's assigned composer.").ThrowOnError();
                Shield.ValueNotNull(value, nameof(LocalizedName)).ThrowOnError();

                _localizedName = value;
            }
        }

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

        public ComposerArticle(Composer composer, ComposerName name, CultureInfo culture):
            this()
        {
            Shield.ArgumentNotNull(composer).ThrowOnError();
            Shield.ArgumentNotNull(culture).ThrowOnError();
            Shield.ArgumentNotNull(name).ThrowOnError();
            Shield.Assert(culture, culture.Equals(name.Language), x => new InvalidOperationException($"The {nameof(culture)} parameter is different than the {nameof(name)} parameter's locale.")).ThrowOnError();

            Composer = composer;
            Language = culture;
            LocalizedName = name;
        }

        public override bool Equals(object obj)
        {
            ComposerArticle other = obj as ComposerArticle;
            if (other == null) return false;

            return
                Language.Equals(other.Language) &&
                IsArchived == other.IsArchived &&
                CreatedUtc == other.CreatedUtc &&
                Composer.Id == other.Composer.Id &&
                StorageId == other.StorageId;
        }

        public override int GetHashCode() => StorageId.GetHashCode();
    }
}
