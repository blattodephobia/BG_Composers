using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class BgcUserManager : UserManager<BgcUser, long>
    {
        public BgcUserManager(IUserStore<BgcUser, long> userStore) :
            base(userStore)
        {

        }
    }
}
