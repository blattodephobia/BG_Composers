using BGC.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational
{
    [Table(nameof(ComposerName))]
    internal class NameRelationalDto : RelationdalDtoBase
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(ComposerRelationalDto))]
        public Guid Composer_Id { get; set; }

        public ComposerRelationalDto Composer { get; set; }

        [MaxLength(5)]
        public string Language { get; set; }

        [MaxLength(128)]
        public string FullName { get; set; }
    }
}
