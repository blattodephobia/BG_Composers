using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class Invitation : BgcEntity<Guid>
    {
        [Required]
        public string Email { get; set; }

        public DateTime ExpirationDate { get; set; }

        [Required]
        public virtual BgcUser Sender { get; set; }

        /// <summary>
        /// Returns true if the <see cref="ExpirationDate"/> is in the future.
        /// </summary>
        public bool IsValid => DateTime.Now < ExpirationDate;

        protected Invitation()
        {
            Id = Guid.NewGuid();
        }

        public Invitation(string email, DateTime expirationDate) :
            this()
        {
            Email = Shield.IsNotNullOrEmpty(email, nameof(email));
            ExpirationDate = expirationDate;
        }
    }
}
