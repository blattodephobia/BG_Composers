using CodeShield;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	public class Composer : BgcEntity<Guid>
	{
        private ICollection<ComposerName> _localizedNames;
        private ICollection<ComposerArticle> _articles;
        private bool _getHashCodeCalled;
        private Guid _id;

        public override Guid Id
        {
            get
            {
                return _id;
            }

            set
            {
                Shield.AssertOperation(this, !_getHashCodeCalled, $"This object's {nameof(Id)} cannot be changed, since {nameof(GetHashCode)} has been called and it relies on the {nameof(Id)}'s value").ThrowOnError();

                _id = value;
            }
        }

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

        public void AddArticle(ComposerArticle article)
        {
            Shield.ArgumentNotNull(article, nameof(article)).ThrowOnError();
            Shield.AssertOperation(article, !article.IsArchived, $"Cannot add an already archived article.").ThrowOnError();

            IEnumerable<ComposerArticle> articlesToArchive = Articles.Where(a => !a.IsArchived && a.Language.Equals(article.Language));
            foreach (ComposerArticle oldArticle in articlesToArchive)
            {
                oldArticle.IsArchived = true;
            }

            Articles.Add(article);
        }

        public IEnumerable<ComposerArticle> GetArticles() => Articles?.Where(a => !a.IsArchived) ?? Enumerable.Empty<ComposerArticle>();

        /// <summary>
        /// Gets a <see cref="ComposerArticle"/> that hasn't been archived in the specified language.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public ComposerArticle GetArticle(CultureInfo language)
        {
            Shield.ArgumentNotNull(language, nameof(language)).ThrowOnError();

            ComposerArticle result = (from article in Articles
                                      where !article.IsArchived && article.Language.Equals(language)
                                      orderby article.CreatedUtc descending
                                      select article).FirstOrDefault();
            return result;
        }

        public ComposerName GetName(CultureInfo language)
        {
            Shield.ArgumentNotNull(language).ThrowOnError();

            ComposerName result = LocalizedNames?.FirstOrDefault(name => name.Language.Equals(language));
            return result;
        }

        public override int GetHashCode()
        {
            _getHashCodeCalled = true;

            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Composer other = obj as Composer;
            return other != null && other.Id == Id;
        }
	}
}
