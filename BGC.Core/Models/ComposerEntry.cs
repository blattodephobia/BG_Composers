using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	public class ComposerEntry : BgcEntity<long>
	{
		public string CultureName { get; set; }

		public Guid ArticleId { get; set; }

		public long? ComposerId { get; set; }

		public virtual Composer Composer { get; set; }

		public long? ComposerNameId { get; set; }

		public virtual ComposerName LocalizedName { get; set; }

		public ComposerEntry()
		{
		}
	}
}
