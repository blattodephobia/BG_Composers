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
    [Table(nameof(ComposerProfile))]
    internal class ProfileRelationalDto : RelationdalDtoBase
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey(nameof(Composer))]
        public Guid Composer_Id { get; set; }

        public ComposerRelationalDto Composer { get; set; }

        [ForeignKey(nameof(ProfilePicture))]
        public int? ProfilePicture_Id { get; set; }

        public MediaTypeInfoRelationalDto ProfilePicture { get; set; }
    }
}
