using BGC.Core.Exceptions;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class ComposerSearchResult : SearchResult
    {
        private ComposerName _name;
        public ComposerName Name
        {
            get
            {
                return _name;
            }

            private set
            {
                _name = value;
                base.Header = value?.FullName;
            }
        }

        public ComposerArticle ArticlePreview { get; private set; }

        public override string Header
        {
            get
            {
                return Name?.FullName;
            }

            set
            {
            }
        }

        public ComposerSearchResult(Composer composer, CultureInfo locale) :
            base(Shield.ArgumentNotNull(composer).GetValueOrThrow().Id)
        {
            Shield.ArgumentNotNull(locale).ThrowOnError();

            Shield.Assert(composer, composer.Name.All().Any(n => n.Key.Equals(locale)), x => new NameNotFoundException(locale, composer.Id)).ThrowOnError();
            Shield.Assert(composer, composer.FindArticle(locale) != null, x => new ArticleNotFoundException(locale, composer.Id)).ThrowOnError();

            Name = composer.Name[locale];
            ArticlePreview = composer.GetArticle(locale);
            Preview = composer.Profile?.ProfilePicture;
        }
    }
}
