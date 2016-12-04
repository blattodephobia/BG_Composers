using BGC.Core.Exceptions;
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

        public BgcUser Create(Guid invitationId)
        {
            BgcUser result = null;
            Invitation invitation = InvitationsRepo.All().FirstOrDefault(i => i.Id == invitationId);
            if (invitation != null)
            {
                result = new BgcUser();
                foreach (BgcRole role in invitation.AvailableRoles)
                {
                    result.Roles.Add(new BgcUserRole() { Role = role, User = result });
                }
            }

            return result;
        }

        public TimeSpan InvitationExpiration { get; set; }

        public IEnumerable<BgcUser> GetUsers() => Users.ToList();

        /// <summary>
        /// Generates an <see cref="Invitation"/> object that will be used to create a new user, once that user has received the invitation in their email.
        /// </summary>
        /// <param name="sender">The user sending the inviation. He or she must have a <see cref="SendInvitePermission"/>.</param>
        /// <param name="email">The email that will be used for the user's account. Use the same one where the invitation will be delivered.</param>
        /// <param name="roles">The roles that the user will participate in, once registered.</param>
        /// <exception cref="UnauthorizedAccessException">The <paramref name="sender"/> doesn't have the necessary permissions to invite others.</exception>
        /// <returns></returns>
        public Invitation Invite(BgcUser sender, string email, IEnumerable<BgcRole> roles)
        {
            Shield.ArgumentNotNull(sender).ThrowOnError();
            Shield.IsNotNullOrEmpty(email).ThrowOnError();
            Shield.IsNotNullOrEmpty(roles).ThrowOnError();
            Shield.Assert(
                value: sender,
                predicate: s => s.FindPermission<SendInvitePermission>() != null,
                exceptionProvider: (s) => new UnauthorizedAccessException($"The user {sender.UserName} does not have permissions to send invites."))
                .ThrowOnError();
            
            if (FindByEmailAsync(email).Result != null)
            {
                throw new DuplicateEntityException($"A user with the email {email} already exists.");
            }

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
