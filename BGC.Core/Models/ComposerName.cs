using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BGC.Core
{
	public class ComposerName : BgcEntity<long>
	{
		private static readonly char[] NamesSeparators = new char[] { ' ' };

		private static List<string> ExtractNames(string value)
		{
			List<string> names = new List<string>(value.Split(NamesSeparators, StringSplitOptions.RemoveEmptyEntries));
			return names;
		}

		private static string CombineNames(IEnumerable<string> names)
		{
			return names.Aggregate(new StringBuilder(), (sb, s) => sb.AppendFormat("{0} ", s), sb => sb.Remove(sb.Length - 1, 1).ToString());
		}

		public long? ComposerId { get; set; }

		public virtual Composer Composer { get; set; }

		public string LocalizationCultureName { get; set; }

		private string firstName;
		public string FirstName
		{
			get
			{
				return this.firstName;
			}

			set
			{
				this.firstName = value;
				if (this.completeName != null)
				{
					List<string> names = ExtractNames(this.completeName);
					names[0] = this.firstName;
					this.completeName = CombineNames(names);
				}
				else
				{
					this.completeName = value;
				}
			}
		}

		private string lastName;
		public string LastName
		{
			get
			{
				return this.lastName;
			}

			set
			{
				if (this.completeName != null)
				{
					List<string> names = ExtractNames(this.completeName);
					this.lastName = value;
					if (names.Count > 1) names[names.Count - 1] = this.lastName;
					else names.Add(value);

					this.completeName = CombineNames(names);
				}
				else
				{
					this.firstName = value;
				}
			}
		}

		private string completeName;
		public string CompleteName
		{
			get
			{
				return this.completeName;
			}

			set
			{
				this.completeName = value;
				List<string> names = ExtractNames(value);
				this.firstName = names[0];
				if (names.Count > 1) this.lastName = names.Last();
			}
		}
	}
}
