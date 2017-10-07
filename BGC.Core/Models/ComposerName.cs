using BGC.Utilities;
using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
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

        private static string CombineNames(IEnumerable<string> names) => names.ToStringAggregate(" ");

        private string _languageInternal;
        [Required, MaxLength(5)]
        internal protected string LanguageInternal
        {
            get
            {
                return _languageInternal;
            }

            set
            {
                _languageInternal = EnsureValid(value);
            }
        }

        private Composer _composer;
        [Required]
        public virtual Composer Composer
        {
            get
            {
                return _composer;
            }

            set
            {
                _composer = EnsureValid(value);
            }
        }
        
        private CultureInfo language;
        [Required, NotMapped]
        public CultureInfo Language
        {
            get
            {
                return language ?? (language = CultureInfo.GetCultureInfo(LanguageInternal));
            }

            set
            {
                LanguageInternal = EnsureValid(value).Name;
            }
        }

		private string firstName;
        [MaxLength(32), Unicode, Index]
		public string FirstName
		{
			get
			{
				return this.firstName;
			}

			set
			{
                EnsureValid(value);
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
        [Required, MaxLength(32), Unicode, Index]
        public string LastName
		{
			get
			{
				return this.lastName;
			}

			set
			{
                EnsureValid(value);
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

        /// <summary>
        /// Gets the full name of the composer in western order ({first_name} {middle_name} {last_name})
        /// </summary>
        [Required, MaxLength(128), Unicode, Index]
        public string FullName
		{
			get
			{
				return this.fullName;
			}

			set
			{
				this.fullName = EnsureValid(value);
				List<string> names = ExtractNames(value);
				this.lastName = names.Last();
				if (names.Count > 1) this.firstName = names.First();
			}
		}

        // This constructor is added to support Entity Framework
        protected ComposerName()
        {
        }

        public ComposerName(string westernOrderFullName, string language) :
            this(westernOrderFullName, CultureInfo.GetCultureInfo(language))
        {
        }

        public ComposerName(string westernOrderFullName, CultureInfo language) :
            this()
        {
            this.FullName = westernOrderFullName.ArgumentNotNull(nameof(westernOrderFullName));
            this.Language = language.ArgumentNotNull(nameof(language));
        }

        /// <summary>
        /// Gets the full name of the composer in Eastern order ({last_name}, {given_name} {middle_name})
        /// </summary>
        /// <returns></returns>
        public string GetEasternOrderFullName()
        {
            List<string> names = ExtractNames(FullName);
            string restOfNames = CombineNames(names.Take(names.Count - 1));
            return names.Last() + (string.IsNullOrEmpty(restOfNames) ? "" : $", {restOfNames}");
        }

        public override string ToString() => this.FullName;
	}
}
