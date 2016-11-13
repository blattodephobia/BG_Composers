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
        private string EncryptToken(string token) => !string.IsNullOrEmpty(EncryptionKey)
            ? Encoding.ASCII.GetBytes(token).Encrypt(EncryptionKey).ToBase62()
            : token;

        private string DecryptToken(string token) => !string.IsNullOrEmpty(token)
                    ? Encoding.ASCII.GetString(token.FromBase62().Decrypt(EncryptionKey))
                    : token;

        public BgcUserManager(IUserStore<BgcUser, long> userStore) :
            base(userStore)
        {
        }

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
                string plainToken = await UserTokenProvider.GenerateAsync(TokenPurposes.ResetPassword, this, user);
                user.SetPasswordResetTokenHash(plainToken);
                await UpdateAsync(user);

                return EncryptToken(plainToken);
            }
            else
            {
                throw new EntityNotFoundException(userId, typeof(BgcUser));
            }
        }

        public async override Task<IdentityResult> ResetPasswordAsync(long userId, string token, string newPassword)
        {
            Shield.ArgumentNotNull(token, nameof(token)).ThrowOnError();
            Shield.ArgumentNotNull(newPassword, nameof(newPassword)).ThrowOnError();

            try
            {
                string plainToken = await Task.Run(() => DecryptToken(token));
                BgcUser user = await FindByIdAsync(userId);

                if (user?.CheckPasswordResetToken(plainToken) ?? false)
                {
                    return await base.ResetPasswordAsync(userId, plainToken, newPassword);
                }
                else
                {
                    return IdentityResult.Failed($"Invalid");
                }
            }
            catch (InvalidDataException)
            {
                return IdentityResult.Failed($"Token contains invalid characters. Valid characters are alphanumerical only.");
            }
            catch (CryptographicException)
            {
                return IdentityResult.Failed($"The token couldn't be decrypted. Either the {nameof(EncryptionKey)} passed is invalid or a corrupt token string was passed.");
            }
            catch (Exception e)
            {
                return IdentityResult.Failed(e.Message);
            }
        }

        public async Task<bool> ValidatePasswordResetToken(BgcUser user, string token)
        {
            Shield.ArgumentNotNull(user, nameof(token)).ThrowOnError();
            Shield.ArgumentNotNull(token, nameof(token)).ThrowOnError();

            bool isValid = false;
            try
            {
                string plainToken = DecryptToken(token);
                isValid = await Task.Run(() => user.CheckPasswordResetToken(token)) &&
                          await UserTokenProvider.ValidateAsync(TokenPurposes.ResetPassword, DecryptToken(token), this, user);
            }
            catch (CryptographicException)
            {
            }

            return isValid;
        }

        /// <summary>
        /// Gets or sets the encryption key used for AES encryption/decryption. Set to null to disable encryption.
        /// </summary>
        public string EncryptionKey { get; set; }
    }
}
