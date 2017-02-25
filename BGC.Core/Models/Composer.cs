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
        private ICollection<ComposerName> _localizedNames;
        private ICollection<ComposerArticle> _articles;

		public virtual ICollection<ComposerName> LocalizedNames
        {
            get
            {
                return _localizedNames ?? (_localizedNames = new HashSet<ComposerName>());
            }

            set
            {
                _localizedNames = value;
                foreach (ComposerName name in _localizedNames ?? Enumerable.Empty<ComposerName>())
                {
                    if (name.Composer == null)
                    {
                        name.Composer = this;
                    }
                    else if (name.Composer != this)
                    {
                        throw new InvalidOperationException("Attempted to add a foreign composer's name");
                    }
                }
            }
        }

		public virtual ICollection<ComposerArticle> Articles
        {
            get
            {
                return _articles ?? (_articles = new HashSet<ComposerArticle>());
            }

            set
            {
                _articles = value;
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
