﻿using BGC.Utilities;
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

        public async override Task<IdentityResult> ResetPasswordAsync(long userId, string token, string newPassword)
        {
            Shield.ArgumentNotNull(token, nameof(token)).ThrowOnError();
            Shield.ArgumentNotNull(newPassword, nameof(newPassword)).ThrowOnError();

            try
            {
                BgcUser user = await FindByIdAsync(userId);

                if (user?.CheckPasswordResetToken(token) ?? false)
                {
                    return await base.ResetPasswordAsync(userId, token, newPassword);
                }
                else
                {
                    return IdentityResult.Failed($"Invalid user or token.");
                }
            }
            catch (InvalidDataException)
            {
                return IdentityResult.Failed($"Token contains invalid characters. Valid characters are alphanumerical only.");
            }
            catch (Exception e)
            {
                return IdentityResult.Failed(e.Message);
            }
        }
    }
}
