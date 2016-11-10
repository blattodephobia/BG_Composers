using BGC.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async override Task<string> GeneratePasswordResetTokenAsync(long userId)
        {
            BgcUser user = await FindByIdAsync(userId);
            if (user != null)
            {
                string plainToken = await UserTokenProvider.GenerateAsync(TokenPurposes.ResetPassword, this, user);
                user.SetPasswordResetTokenHash(plainToken);
                await UpdateAsync(user);

                string result = !string.IsNullOrEmpty(EncryptionKey)
                    ? Encoding.ASCII.GetBytes(plainToken).Encrypt(EncryptionKey).ToBase62()
                    : plainToken;
                return result;
            }
            else
            {
                throw new EntityNotFoundException(userId, typeof(BgcUser));
            }
        }

        public override Task<IdentityResult> ResetPasswordAsync(long userId, string token, string newPassword)
        {
            try
            {
                string plainToken = !string.IsNullOrEmpty(token)
                    ? Encoding.ASCII.GetString(token.FromBase62().Decrypt(EncryptionKey))
                    : token;

                return base.ResetPasswordAsync(userId, plainToken, newPassword);
            }
            catch (InvalidDataException)
            {
                throw new InvalidDataException($"Token contains invalid characters. Valid characters are alphanumerical only.");
            }
        }

        public string EncryptionKey { get; set; }
    }
}
