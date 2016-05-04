using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	public class Composer : BgcEntity<long>
	{
		public virtual ICollection<ComposerName> LocalizedNames { get; set; }

		public virtual ICollection<ComposerEntry> Articles { get; set; }

		public Composer()
		{
		}
	}
}
