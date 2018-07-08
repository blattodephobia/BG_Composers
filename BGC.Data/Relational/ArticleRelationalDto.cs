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
    public class ArticleRelationalDto : RelationdalDtoBase
    {
        [Key]
        public long Id { get; set; }

        [Index]
		public Guid StorageId { get; set; }

        [Required]
        [ForeignKey(nameof(Composer))]
        public Guid Composer_Id { get; set; }

        public virtual ComposerRelationalDto Composer { get; set; }

        [Required]
        [MaxLength(5)]
        public string Language { get; set; }

        public DateTime CreatedUtc { get; set; }

        public bool IsArchived { get; set; }

        public virtual ICollection<ArticleMediaRelationalDto> Media { get; set; }

        internal protected ArticleRelationalDto()
        {
        }

        public override int GetHashCode() => Id.GetHashCode();

        public override bool Equals(object obj)
        {
            ArticleRelationalDto other = obj as ArticleRelationalDto;
            return
                Id == other?.Id &&
                StorageId == other?.StorageId &&
                Composer_Id == other?.Composer_Id &&
                Language == other?.Language &&
                CreatedUtc == other?.CreatedUtc &&
                IsArchived == other?.IsArchived &&
                (Media?.SequenceEqual(other?.Media ?? Enumerable.Empty<ArticleMediaRelationalDto>()) ?? (Media == null && other?.Media == null));
        }
    }
}
