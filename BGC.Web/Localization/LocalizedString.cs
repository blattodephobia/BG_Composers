using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.WebAPI
{
	public class LocalizedString
	{
		public static implicit operator string(LocalizedString _string)
		{
			return _string.Value;
		}

		public string Key { get; private set; }

		private string value;
		public string Value
		{
			get
			{
				if (value == null)
				{
					this.LocalizedStrings.TryGetValue(this.Key, out this.value);
				}

				this.value = this.value ?? string.Empty;
				return this.value;
			}
		}

		public LocalizedString(string key, IDictionary<string, string> localizedStrings)
		{
			this.Key = key;
			this.LocalizedStrings = localizedStrings;
		}

		public override string ToString()
		{
			return this.Value;
		}

		private IDictionary<string, string> LocalizedStrings { get; set; }
	}
}