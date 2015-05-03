using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.WebAPI.Models
{
	public class LocalizedString
	{
		public string Key { get; private set; }
		public string Value { get; private set; }

		public static implicit operator string(LocalizedString _string)
		{
			return _string.Value;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", this.Key, this.Value);
		}
	}
}