using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	public abstract class BgcEntity<TKey>
	{
		[Key]
		public virtual TKey Id { get; set; }
	}
}
