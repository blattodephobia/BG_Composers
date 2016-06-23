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
	public class ComposerArticle : BgcEntity<long>
	{
        private CultureInfo language;

		internal protected string LanguageInternal
        {
            get
            {
                return this.language.Name;
            }

            set
            {
                this.language = CultureInfo.GetCultureInfo(value.ValueNotNull(nameof(Language)).GetValueOrThrow());
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
                this.language = value.ValueNotNull(nameof(Language)).GetValueOrThrow();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Guid"/> that identifies the actual text of this article, as stored in an external service (such as <see cref="IDataStorageService"/>).
        /// </summary>
		public Guid StorageId { get; set; }

		public long ComposerNameId { get; set; }

        [Required]
		public virtual ComposerName LocalizedName { get; set; }

		public ComposerArticle()
		{
		}
	}
}
