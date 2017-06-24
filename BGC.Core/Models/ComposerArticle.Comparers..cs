using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public partial class ComposerArticle
    {
        public static class Comparers
        {
            /// <summary>
            /// Returns an <see cref="IEqualityComparer{T}"/> that identifies two <see cref="ComposerArticle"/>s as equal when they refer
            /// to the same composer.
            /// </summary>
            public static readonly IEqualityComparer<ComposerArticle> ByComposerEqualityComparer = new ByComposerEqualityComparer();
        }
    }
}
