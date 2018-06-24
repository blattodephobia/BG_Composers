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
    [Table(nameof(ComposerArticle))]
    internal class ArticleRelationalDto : RelationdalDtoBase
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(5)]
        public string Language { get; set; }

        public DateTime CreatedUtc { get; set; }

        [Index]
		public Guid StorageId { get; set; }

        [Required]
        [ForeignKey(nameof(Composer))]
        public Guid Composer_Id { get; set; }

        public ComposerRelationalDto Composer { get; set; }

        public bool IsArchived { get; set; }

        public virtual ICollection<ArticleMediaRelationalDto> Media { get; set; }
    }
}
