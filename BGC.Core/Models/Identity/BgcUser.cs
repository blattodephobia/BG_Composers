using BGC.Utilities;
using CodeShield;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class BgcUser : IdentityUser<long, BgcUserLogin, BgcUserRole, BgcUserClaim>
    {
        internal const int PASSWORD_RESET_TOKEN_LENGTH = 32;

        public static readonly string AdministratorUserName = "Administrator";

        public bool MustChangePassword { get; set; }

        /// <summary>
        /// Gets or sets the hash of a password reset token in the form of a Base64 string.
        /// </summary>
        [Column(TypeName = "varchar")]
        [StringLength((int)((4d * PASSWORD_RESET_TOKEN_LENGTH / 3) / 4 + 1 - double.Epsilon) * 4)]
        /* Compute the maximum length of the Base64 string that can be generated from PASSWORD_RESET_TOKEN_LENGTH number of bytes.
         * Base64 strings' length is obtained by multiplying the length of the input array by 4/3, then rounding up the result to the nearest
         * value divisible by 4. The formula above does the following:
         * 1. Compute 4/3 * num bytes. This is the value that has to be exactly divisible by four;
         *    if not, we have to round up so that it is exactly divisible by four.
         * 2. Divide the result by 4 and add 0.(9).
         * 3.1 If the result was exactly divisible by four, we need to obtain that result back. So, we cast to int, getting rid of
         *     the fractional part, and multiply back by four.
         * 3.2 If the result wasn't exactly divisible by four, we now have a number whose integral part is equal to the required string length.
         *     Once again, all we have to do is to get rid of the fractional part (by casting to int), then multiply by four.
         * 
         * Thus the formula is correct. In addition, it can be rewritten as (PASSWORD_RESET_TOKEN_LENGTH / 3d + 1 - double.Epsilon) * 4, but
         * in that case, the intent isn't as obvious.
         */
        public string PasswordResetTokenHash { get; set; }

        public virtual ICollection<Setting> UserSettings { get; set; }

		public BgcUser()
		{
		}

		public BgcUser(string username)
		{
			UserName = username;
		}

        public IEnumerable<Permission> GetPermissions()
        {
            return Roles.SelectMany(userRole => userRole.Role?.Permissions ?? Enumerable.Empty<Permission>());
        }

        public T FindPermission<T>() where T : Permission
        {
            T result = GetPermissions().OfType<T>().FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Computes the SHA-256 hash of a plain-text token, converts it to a Base64 string and sets it to the <see cref="PasswordResetTokenHash"/> property. 
        /// </summary>
        /// <param name="plainToken"></param>
        public void SetPasswordResetTokenHash(string plainToken)
        {
            byte[] hashCode = plainToken.GetHashCode<SHA256Managed>();
            PasswordResetTokenHash = Convert.ToBase64String(hashCode);
        }

        /// <summary>
        /// Determines whether the supplied's token hash matches the one stored in <see cref="PasswordResetTokenHash"/>. 
        /// </summary>
        /// <param name="plainToken"></param>
        /// <returns>True, if the plain token's hash matches the stored password reset token hash. False when they don't match or there is no stored hash.</returns>
        public bool CheckPasswordResetToken(string plainToken)
        {
            Shield.IsNotNullOrEmpty(plainToken, nameof(plainToken)).ThrowOnError();
            return PasswordResetTokenHash != null
                ? Convert.ToBase64String(plainToken.GetHashCode<SHA256Managed>()) == PasswordResetTokenHash
                : false;
        }
	}
}
