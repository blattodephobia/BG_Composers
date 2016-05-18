using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class UserSetting : Setting
    {
        public long UserId { get; set; }

        [Required]
        public virtual AspNetUser User { get; set; }
    }
}
