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
        [Key]
        public Guid Id { get; set; }

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

        [ForeignKey(nameof(Profile))]
        public int? Profile_Id { get; set; }

        public virtual ProfileRelationalDto Profile { get; set; }

        private ICollection<NameRelationalDto> _localizedNames;
        public virtual ICollection<NameRelationalDto> LocalizedNames
        {
            get
            {
                return _localizedNames ?? (_localizedNames = new HashSet<NameRelationalDto>());
            }

            set
            {
                _localizedNames = value;
            }
        }

        private ICollection<ArticleRelationalDto> _articles;
        public virtual ICollection<ArticleRelationalDto> Articles
        {
            get
            {
                return _articles ?? (_articles = new HashSet<ArticleRelationalDto>());
            }

            set
            {
                _articles = value;
            }
        }
    }
}
