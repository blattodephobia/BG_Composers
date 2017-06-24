using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public partial class ComposerArticle : BgcEntity<long>
    {
        private class ByComposerEqualityComparer : IEqualityComparer<ComposerArticle>
        {
            public bool Equals(ComposerArticle x, ComposerArticle y)
            {
                Shield.ArgumentNotNull(x).ThrowOnError();
                Shield.ArgumentNotNull(y).ThrowOnError();

                Shield.ValueNotNull(x.Composer, $"{nameof(ComposerArticle)}.{nameof(x.Composer)}").ThrowOnError();
                Shield.ValueNotNull(y.Composer, $"{nameof(ComposerArticle)}.{nameof(y.Composer)}").ThrowOnError();

                return x.Composer.Equals(y.Composer);
            }

            public int GetHashCode(ComposerArticle obj)
            {
                Shield.ArgumentNotNull(obj, nameof(obj)).ThrowOnError();
                Shield.ValueNotNull(obj.Composer).ThrowOnError();

                return obj.Composer.GetHashCode();
            }
        }
    }
}