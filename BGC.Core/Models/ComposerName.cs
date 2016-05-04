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
				if (this.fullName != null)
				{
					List<string> names = ExtractNames(this.fullName);
					names[0] = this.firstName;
					this.fullName = CombineNames(names);
				}
				else
				{
					this.fullName = value;
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
				if (this.fullName != null)
				{
					List<string> names = ExtractNames(this.fullName);
					this.lastName = value;
					if (names.Count > 1) names[names.Count - 1] = this.lastName;
					else names.Add(value);

					this.fullName = CombineNames(names);
				}
				else
				{
					this.firstName = value;
				}
			}
		}

		private string fullName;
		public string FullName
		{
			get
			{
				return this.fullName;
			}

			set
			{
				this.fullName = value;
				List<string> names = ExtractNames(value);
				this.firstName = names[0];
				if (names.Count > 1) this.lastName = names.Last();
			}
		}

        public ComposerName()
        {
        }

        public override string ToString() => this.FullName;

        public ComposerName(string completeName) :
            this()
        {
            this.FullName = completeName;
        }
	}
}
