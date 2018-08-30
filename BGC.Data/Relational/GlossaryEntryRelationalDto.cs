using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational
{
    public class GlossaryEntryRelationalDto : RelationdalDtoBase, IGlossaryEntryDto
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public Guid DomainId { get; set; }

        private ICollection<GlossaryDefinitionRelationalDto> _translations;
        public virtual ICollection<GlossaryDefinitionRelationalDto> Translations
        {
            get
            {
                return _translations ?? (_translations = new HashSet<GlossaryDefinitionRelationalDto>());
            }

            set
            {
                _translations = value;
            }
        }

        [NotMapped]
        Guid IGlossaryEntryDto.Id => DomainId;

        internal protected GlossaryEntryRelationalDto()
        {
        }
    }
}
