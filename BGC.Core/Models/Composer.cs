using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	public class Composer : BgcEntity<long>
	{
        private ICollection<ComposerName> localizedNames;
        private ICollection<ComposerEntry> articles;

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

		public virtual ICollection<ComposerEntry> Articles
        {
            get
            {
                return this.articles ?? (this.articles = new HashSet<ComposerEntry>());
            }

            set
            {
                this.articles = value;
            }
        }

		public Composer()
		{
		}
	}
}
