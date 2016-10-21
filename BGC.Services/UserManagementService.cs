using BGC.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BGC.Core;
using CodeShield;
using Microsoft.AspNet.Identity;

namespace BGC.Services
{
    internal class UserManagementService : DbServiceBase, IUserManagementService
    {
        private IRepository<BgcUser> UsersRepo { get; set; }
        private IRepository<Invitation> InvitationsRepo { get; set; }
        private IRepository<BgcRole> RolesRepo { get; set; }

        private BgcUser administrator;
        public BgcUser Administrator
        {
            get
            {
                return this.administrator;
            }
            set
            {
                this.administrator = value.ValueNotNull(nameof(Administrator)).And(
                    Shield.Assert(value, user => user.FindPermission<SendInvitePermission>() != null, user => new InvalidOperationException($"The user {user.UserName} is not authorized to send invites."))).GetValueOrThrow();
            }
        }

        public TimeSpan InvitationExpiration { get; set; }

        public IEnumerable<BgcUser> GetUsers()
        {
            return UsersRepo.All().ToList();
        }

        public Invitation Invite(string email, IEnumerable<BgcRole> roles)
        {
            var matchingRoles = from role in RolesRepo.All()
                                join reqRole in from name in roles
                                                select name.Name 
                                                on role.Name equals reqRole
                                select role;
            Invitation result = new Invitation(email, DateTime.Now.Add(InvitationExpiration))
            {
                Sender = Administrator,
                AvailableRoles = new HashSet<BgcRole>(matchingRoles)
            };

            InvitationsRepo.Insert(result);
            InvitationsRepo.UnitOfWork.SaveChanges();
            return result;
        }

        public UserManagementService(IRepository<BgcUser> usersRepo, IRepository<Invitation> invitationsRepo, IRepository<BgcRole> roleStore, BgcUser admin)
        {
            UsersRepo = usersRepo.ArgumentNotNull(nameof(usersRepo)).GetValueOrThrow();
            InvitationsRepo = invitationsRepo.ArgumentNotNull(nameof(invitationsRepo)).GetValueOrThrow();
            Administrator = Shield.ArgumentNotNull(admin, nameof(admin)).And(
                Shield.Assert(admin, user => user.FindPermission<SendInvitePermission>() != null, user => new InvalidOperationException($"The user {user.UserName} is not authorized to send invites."))).GetValueOrThrow();
            RolesRepo = Shield.ArgumentNotNull(roleStore).GetValueOrThrow();
        }
    }
}
