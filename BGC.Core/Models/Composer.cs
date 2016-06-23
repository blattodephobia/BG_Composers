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
            if (result == null)
            {
                throw new InvalidOperationException($"There is no article in {language.EnglishName} for composer with Id = {Id}.");
            }

            return result;
        }

		public Composer()
		{
		}
	}
}
