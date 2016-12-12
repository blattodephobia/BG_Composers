using CodeShield;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	public class Composer : BgcEntity<long>
	{
        private ICollection<ComposerName> localizedNames;
        private ICollection<ComposerArticle> articles;

		public virtual ICollection<ComposerName> LocalizedNames
        {
            get
            {
                return this.localizedNames ?? (this.localizedNames = new HashSet<ComposerName>());
            }

            set
            {
                this.localizedNames = value;
            }
        }

		public virtual ICollection<ComposerArticle> Articles
        {
            get
            {
                return this.articles ?? (this.articles = new HashSet<ComposerArticle>());
            }

            set
            {
                this.articles = value;
            }
        }

        public ComposerArticle GetArticle(CultureInfo language)
        {
            Shield.ArgumentNotNull(language, nameof(language)).ThrowOnError();

            ComposerArticle result = Articles.FirstOrDefault(article => article.LanguageInternal == language.Name);
            return result;
        }

        public ComposerName GetName(CultureInfo language)
        {
            Shield.ArgumentNotNull(language).ThrowOnError();

            ComposerName result = LocalizedNames?.FirstOrDefault(name => name.Language == language);
            return result;
        }

		public Composer()
		{
		}
	}
}
