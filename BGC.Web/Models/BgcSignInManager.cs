using BGC.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BGC.Web.Models
{
    public class BgcSignInManager : SignInManager<BgcUser, long>
    {
        public BgcSignInManager(UserManager<BgcUser, long> userManager, IAuthenticationManager authentication) :
            base(userManager, authentication)
        {
        }

        public override async Task<ClaimsIdentity> CreateUserIdentityAsync(BgcUser user)
        {
            ClaimsIdentity id = await base.CreateUserIdentityAsync(user);
            id.AddClaims(user.GetPermissions().Select(p => new Claim(nameof(IPermission), p.Name)));
            return id;
        }
    }
}