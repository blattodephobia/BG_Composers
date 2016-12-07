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
        private static readonly TimeSpan DefaultInvitationExpiration = TimeSpan.FromDays(1);

        public BgcUserManager(IUserStore<BgcUser, long> userStore, IRepository<BgcRole> roleRepository, IRepository<Invitation> invitationsRepo) :
            base(userStore)
        {
            Shield.ArgumentNotNull(userStore).ThrowOnError();
            Shield.ArgumentNotNull(roleRepository).ThrowOnError();
            Shield.ArgumentNotNull(invitationsRepo).ThrowOnError();

            RoleRepo = roleRepository;
            InvitationsRepo = invitationsRepo;
            InvitationExpiration = DefaultInvitationExpiration;
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

        /// <summary>
        /// Creates a new user from the specified invitation id.
        /// </summary>
        /// <param name="invitationId">The id of an invitation stored in the database.</param>
        /// <returns>A new <see cref="BgcUser"/> instance with </returns>
        public BgcUser Create(Guid invitationId, string userName, string password)
        {
            Shield.IsNotNullOrEmpty(password, nameof(invitationId));
            if (FindByNameAsync(userName).Result != null)
            {
                throw new DuplicateEntityException();
            }

            Invitation invitation = InvitationsRepo.All().FirstOrDefault(i => i.Id == invitationId);
            if (invitation != null)
            {
                BgcUser result = new BgcUser(userName)
                {
                    Email = invitation.Email,
                    EmailConfirmed = true
                };
                result.Roles.AddRange(invitation.AvailableRoles.Select(role => new BgcUserRole() { Role = role, User = result }));
                CreateAsync(result, password).Wait();

                InvitationsRepo.Delete(invitation);
                InvitationsRepo.UnitOfWork.SaveChanges();
                return result;
            }
            else
            {
                throw new EntityNotFoundException(invitationId, typeof(Invitation));
            }
        }

        /// <summary>
        /// Searches the underlying repository for an invitation with the given id and returns it.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An <see cref="Invitation"/> instance if there is an invitation with the given id. Null otherwise.</returns>
        public Invitation FindInvitation(Guid id)
        {
            DateTime utcNow = DateTime.UtcNow;
            Invitation result = InvitationsRepo.All().FirstOrDefault(i => i.Id == id && i.ExpirationDate > utcNow);
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
        /// <exception cref="DuplicateEntityException">A user with the same email address is already registered in the system.</exception>
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
            Shield.Assert(email, e => FindByEmailAsync(email).Result == null, e => new DuplicateEntityException($"A user with the email {e} already exists.")).ThrowOnError();

            Invitation oldInvitation = InvitationsRepo.All().FirstOrDefault(i => i.Email == email);
            if (oldInvitation != null)
            {
                InvitationsRepo.Delete(oldInvitation);
            }

            var matchingRoles = from role in RoleRepo.All()
                                join reqRole in from name in roles
                                                select name.Name
                                                on role.Name equals reqRole
                                select role;
            Invitation result = new Invitation(email, DateTime.UtcNow.Add(InvitationExpiration))
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
