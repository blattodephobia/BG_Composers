using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Models
{
    public class GlossaryDefinition : BgcEntity<int>
    {
        private CultureInfo _language;
        private string _definition;
        private string _term;

        [Required]
        internal protected string LanguageInternal
        {
            get
            {
                return _language.Name;
            }

            set
            {                
                _language = CultureInfo.GetCultureInfo(value);
            }
        }

        [NotMapped]
        public CultureInfo Language
        {
            get
            {
                return _language;
            }

            set
            {
                
                _language = value;
            }
        }

        [Required, Unicode]
        public string Term
        {
            get
            {
                return _term;
            }

            set
            {
                Shield.IsNotNullOrEmpty(value, nameof(Term)).ThrowOnError();

                _term = value;
            }
        }

        [Required, Unicode]
        public string Definition
        {
            get
            {
                return _definition;
            }

            set
            {
                Shield.IsNotNullOrEmpty(value).ThrowOnError();

                _definition = value;
            }
        }

        protected GlossaryDefinition()
        {
        }

        public GlossaryDefinition(CultureInfo language, string definition, string term) :
            this()
        {
            Shield.ArgumentNotNull(language, nameof(language)).ThrowOnError();
            Shield.IsNotNullOrEmpty(definition, nameof(definition)).ThrowOnError();
            Shield.IsNotNullOrEmpty(term, nameof(term)).ThrowOnError();

            _language = language;
            _definition = definition;
            _term = term;
        }
    }
}
