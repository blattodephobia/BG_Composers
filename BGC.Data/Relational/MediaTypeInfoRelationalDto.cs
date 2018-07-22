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
    [Table(nameof(MediaTypeInfo))]
    [Identity(nameof(StorageId))]
    public class MediaTypeInfoRelationalDto : RelationdalDtoBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MimeType { get; set; }

        [Index]
        public Guid StorageId { get; set; }

        public string OriginalFileName { get; set; }

        public string ExternalLocation { get; set; }

        internal protected MediaTypeInfoRelationalDto()
        {
        }

        public override int GetHashCode() => Id.GetHashCode();

        public override bool Equals(object obj)
        {
            MediaTypeInfoRelationalDto other = obj as MediaTypeInfoRelationalDto;
            return
                Id == other?.Id &&
                MimeType == other?.MimeType &&
                StorageId == other?.StorageId &&
                OriginalFileName == other?.OriginalFileName &&
                ExternalLocation == other?.ExternalLocation;
        }
    }
}
