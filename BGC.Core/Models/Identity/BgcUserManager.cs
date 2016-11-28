using BGC.Utilities;
using CodeShield;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class BgcUserManager : UserManager<BgcUser, long>
    {
        public BgcUserManager(IUserStore<BgcUser, long> userStore, IRepository<BgcRole> roleRepository, IRepository<Invitation> invitationsRepo) :
            base(userStore)
        {
            Shield.ArgumentNotNull(userStore).ThrowOnError();
            Shield.ArgumentNotNull(roleRepository).ThrowOnError();
            Shield.ArgumentNotNull(invitationsRepo).ThrowOnError();

            RoleRepo = roleRepository;
            InvitationsRepo = invitationsRepo;
        }

        protected IRepository<BgcRole> RoleRepo { get; private set; }

        protected IRepository<Invitation> InvitationsRepo { get; private set; }

        /// <summary>
        /// Calls the underlying <see cref="IUserTokenProvider{TUser, TKey}"/> for the ResetPassword purpose. The resulting token's hash is stored
        /// in the <see cref="BgcUser"/> entity and an encrypted token is returned.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async override Task<string> GeneratePasswordResetTokenAsync(long userId)
        {
            BgcUser user = await FindByIdAsync(userId);
            if (user != null)
            {
                string token = await UserTokenProvider.GenerateAsync(TokenPurposes.ResetPassword, this, user);
                user.SetPasswordResetTokenHash(token);
                await UpdateAsync(user);

                return token;
            }
            else
            {
                throw new EntityNotFoundException(userId, typeof(BgcUser));
            }
        }

        public TimeSpan InvitationExpiration { get; set; }

        public IEnumerable<BgcUser> GetUsers() => Users.ToList();

        public Invitation Invite(BgcUser sender, string email, IEnumerable<BgcRole> roles)
        {
            Shield.ArgumentNotNull(sender).ThrowOnError();
            Shield.IsNotNullOrEmpty(email).ThrowOnError();
            Shield.IsNotNullOrEmpty(roles).ThrowOnError();
            Shield.AssertOperation(sender, s => s.FindPermission<SendInvitePermission>() != null, $"The user {sender.UserName} does not have permissions to send invites.").ThrowOnError();

            var matchingRoles = from role in RoleRepo.All()
                                join reqRole in from name in roles
                                                select name.Name
                                                on role.Name equals reqRole
                                select role;
            Invitation result = new Invitation(email, DateTime.Now.Add(InvitationExpiration))
            {
                Sender = sender,
                AvailableRoles = new HashSet<BgcRole>(matchingRoles)
            };

            InvitationsRepo.Insert(result);
            InvitationsRepo.UnitOfWork.SaveChanges();
            return result;
        }
    }
}
