using BGC.Core.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	public class BgcUserRole : IdentityUserRole<long>
	{
        public virtual BgcUser User { get; set; }

        public virtual BgcRole Role { get; set; }
    }
}
