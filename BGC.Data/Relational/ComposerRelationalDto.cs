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
    [Table(nameof(Composer))]
    internal class ComposerRelationalDto : RelationdalDtoBase
    {
        public Guid Id { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfDeath { get; set; }

        /// <summary>
        /// Specifies how many other composers with the same name have been added before this one.
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Order { get; set; }

        public bool HasNamesakes { get; set; }

        [ForeignKey(nameof(Profile))]
        public int? Profile_Id { get; set; }

        public virtual ProfileRelationalDto Profile { get; set; }
    }
}
