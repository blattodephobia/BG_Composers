using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational
{
    public class GlossaryDefinitionRelationalDto : RelationdalDtoBase
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Entry))]
        public int GlossaryEntry_Id { get; set; }

        [Required]
        public virtual GlossaryEntryRelationalDto Entry { get; set; }

        [Required]
        public string TermTranslation { get; set; }

        [Required]
        public string DefinitionTranslation { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
