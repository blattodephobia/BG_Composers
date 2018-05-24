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
    internal class MediaTypeInfoRelationalDto : RelationdalDtoBase
    {
        [Key]
        public int Id { get; set; }

        public string MimeType { get; set; }

        [Index]
        public Guid StorageId { get; set; }

        public string OriginalFileName { get; set; }

        public string ExternalLocation { get; set; }
    }
}
