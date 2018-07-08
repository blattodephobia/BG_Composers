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
    public class ComposerRelationalDto : RelationdalDtoBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public virtual ICollection<NameRelationalDto> LocalizedNames { get; set; } = new HashSet<NameRelationalDto>();

        public virtual ICollection<ArticleRelationalDto> Articles { get; set; } = new HashSet<ArticleRelationalDto>();

        /* Although the Composer entity's DateAdded property is nullable, the DTO is expected to always have a value in a normal workflow.
         * To prevent complications from an additional nullable property, a special DateTime value should be used to identify nulls. This
         * means either default(DateTime), DateTime.MaxValue, DateTime.MinValue; the implementation is up to the property mapper.
         * */
        public DateTime DateAdded { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfDeath { get; set; }

        /// <summary>
        /// Specifies how many other composers with the same name have been added before this one.
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Order { get; set; }

        [ForeignKey(nameof(ProfilePicture))]
        public int? ProfilPicture_Id { get; set; }

        public virtual MediaTypeInfoRelationalDto ProfilePicture { get; set; }

        public virtual ICollection<MediaTypeInfoRelationalDto> Media { get; set; } = new HashSet<MediaTypeInfoRelationalDto>();

        internal protected ComposerRelationalDto()
        {

        }
    }
}
