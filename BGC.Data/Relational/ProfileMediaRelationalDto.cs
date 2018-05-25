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
    [Table(nameof(ComposerProfile) + "_" + nameof(MediaTypeInfo))]
    internal class ProfileMediaRelationalDto : RelationdalDtoBase
    {
        [Key, Column(Order = 0)]
        [ForeignKey(nameof(Profile))]
        public int Profile_Id { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey(nameof(Media))]
        public int Media_Id { get; set; }

        public ProfileRelationalDto Profile { get; set; }

        public MediaTypeInfoRelationalDto Media { get; set; }
    }
}
