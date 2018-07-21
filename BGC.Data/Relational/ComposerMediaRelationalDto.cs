using BGC.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational
{
    public class ComposerMediaRelationalDto : RelationdalDtoBase, IIntermediateRelationalDto<ComposerRelationalDto, MediaTypeInfoRelationalDto>
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey(nameof(Composer))]
        public Guid Composer_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey(nameof(MediaEntry))]
        public int MediaTypeInfo_Id { get; set; }

        public ComposerRelationalDto Composer { get; set; }

        public MediaTypeInfoRelationalDto MediaEntry { get; set; }

        public string Purpose { get; set; }

        internal protected ComposerMediaRelationalDto()
        {
        }

        public override int GetHashCode()
        {
            return HashExtensions.CombineHashCodes(Composer_Id.GetHashCode(), MediaTypeInfo_Id.GetHashCode());
        }

        [Obsolete("Move such logic in a separate IEqualityComparer, otherwise a StackOverflowException might occur when principal DTOs invoke Equals() on this object.")]
        public override bool Equals(object obj)
        {
            ComposerMediaRelationalDto other = obj as ComposerMediaRelationalDto;
            return
                ((Composer?.Equals(other?.Composer) ?? false) || (Composer == null && other != null && other.Composer == null)) &&
                ((MediaEntry?.Equals(other?.MediaEntry) ?? false) || (MediaEntry == null && other != null && other.Composer == null)) &&
                Purpose == other?.Purpose;
        }

        internal static ComposerMediaRelationalDto Create(ComposerRelationalDto composer, MediaTypeInfoRelationalDto media, string purpose = null)
        {
            return new ComposerMediaRelationalDto() { Composer = composer, MediaEntry = media, Purpose = purpose };
        }
    }
}
