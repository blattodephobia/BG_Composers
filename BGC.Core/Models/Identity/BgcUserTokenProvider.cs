using BGC.Utilities;
using CodeShield;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public partial class BgcUserTokenProvider : IUserTokenProvider<BgcUser, long>
    {
        private static readonly int TokenSaltLength = 20;

        private static readonly RandomNumberGenerator RandomNumberService = RandomNumberGenerator.Create();

        private static readonly string[] ValidPurposes = (from field in typeof(TokenPurposes).GetFields(BindingFlags.Static | BindingFlags.Public)
                                                          where field.FieldType == typeof(string)
                                                          select field.GetValue(null) as string).ToArray();

        private static void VerifyPurposeIsValid(string purpose)
        {
            Shield.AssertOperation(purpose, p => ValidPurposes.Contains(purpose), $"The purpose '{purpose}' is not supported.");
        }

        public static readonly TimeSpan DefaultTokenExpiration = TimeSpan.FromHours(24);

        private string EncryptToken(byte[] token) => !string.IsNullOrEmpty(EncryptionKey)
            ? token.Encrypt(EncryptionKey).ToBase62()
            : token.ToBase62();

        private byte[] DecryptToken(string token) => !string.IsNullOrEmpty(EncryptionKey)
            ? token.FromBase62().Decrypt(EncryptionKey)
            : token.FromBase62();

        public BgcUserTokenProvider()
        {
            TokenExpiration = DefaultTokenExpiration;
        }

        /// <summary>
        /// Gets or sets the time span in which generated tokens are valid. The default is 24 hours.
        /// </summary>
        public TimeSpan TokenExpiration { get; set; }
        
        /// <summary>
        /// Gets or sets the encryption key used for AES encryption/decryption. Set to null to disable encryption.
        /// </summary>
        public string EncryptionKey { get; set; }

        public string Generate(string purpose, UserManager<BgcUser, long> manager, BgcUser user)
        {
            Shield.ArgumentNotNull(purpose, nameof(purpose)).ThrowOnError();
            Shield.ArgumentNotNull(manager, nameof(manager)).ThrowOnError();
            Shield.ArgumentNotNull(user, nameof(user)).ThrowOnError();
            Shield.AssertOperation(user, u => IsValidProviderForUser(manager, user), $"This {nameof(BgcUserTokenProvider)} is not a valid provider for user {user.UserName}.").ThrowOnError();
            VerifyPurposeIsValid(purpose);

            if (purpose == TokenPurposes.ResetPassword)
            {
                using (var writer = new BinaryWriter(new MemoryStream()))
                {
                    long validity = DateTime.UtcNow.Add(TokenExpiration).Ticks;
                    long userId = user.Id;
                    writer.Write(validity);
                    writer.Write(userId);
                    writer.Write(TokenSaltLength);
                    byte[] randomBytes = new byte[TokenSaltLength];
                    RandomNumberService.GetBytes(randomBytes);  // make sure no two invocations of the method with the same parameters return identical tokens, otherwise there are security risks
                    writer.Write(randomBytes);
                    writer.Write(user.PasswordHash);
                    byte[] result = (writer.BaseStream as MemoryStream).ToArray();
                    user.SetPasswordResetTokenHash(result.ToBase62());
                    return EncryptToken(result);
                }
            }

            throw new InvalidOperationException();
        }

        public bool IsValidProviderForUser(UserManager<BgcUser, long> manager, BgcUser user)
        {
            Shield.ArgumentNotNull(manager, nameof(manager)).ThrowOnError();
            Shield.ArgumentNotNull(user, nameof(user)).ThrowOnError();

            return manager.FindById(user.Id) != null &&
                   !string.IsNullOrWhiteSpace(user.Email) &&
                   !string.IsNullOrWhiteSpace(user.PasswordHash);
        }

        public void Notify(string token, UserManager<BgcUser, long> manager, BgcUser user)
        {
        }

        public bool Validate(string purpose, string token, UserManager<BgcUser, long> manager, BgcUser user)
        {
            Shield.ArgumentNotNull(purpose, nameof(purpose)).ThrowOnError();
            Shield.ArgumentNotNull(manager, nameof(manager)).ThrowOnError();
            Shield.ArgumentNotNull(user, nameof(user)).ThrowOnError();
            Shield.AssertOperation(user, u => IsValidProviderForUser(manager, user), $"This {nameof(BgcUserTokenProvider)} is not a valid provider for user {user.UserName}.").ThrowOnError();
            VerifyPurposeIsValid(purpose);

            bool isValid = false;
            try
            {
                if (purpose == TokenPurposes.ResetPassword)
                {
                    byte[] decryptedToken = DecryptToken(token);
                    using (var reader = new BinaryReader(new MemoryStream(decryptedToken)))
                    {
                        DateTime validity = new DateTime(ticks: reader.ReadInt64());
                        long userId = reader.ReadInt64();
                        int tokenSaltLength = reader.ReadInt32();
                        reader.ReadBytes(tokenSaltLength); // skip the pseudorandom bytes
                        string passwordHash = reader.ReadString();
                        isValid =
                            DateTime.UtcNow < validity &&
                            userId == user.Id &&
                            passwordHash == user.PasswordHash &&
                            user.CheckPasswordResetToken(decryptedToken.ToBase62());
                    }
                }
            }
            catch (InvalidDataException) // Token contains invalid characters. Valid characters for Base62 encoded strings are alphanumerical only.
            {   
            }
            catch (CryptographicException) // The token couldn't be decrypted. Probably it was encrypted with a different encryption key or a corrupt token string was passed.
            {
            }

            return isValid;
        }

        public Task<string> GenerateAsync(string purpose, UserManager<BgcUser, long> manager, BgcUser user)
            => Task.Run(()
            => Generate(purpose, manager, user));

        public Task<bool> IsValidProviderForUserAsync(UserManager<BgcUser, long> manager, BgcUser user)
            => new Task<bool>(()
            => IsValidProviderForUser(manager, user));

        public Task NotifyAsync(string token, UserManager<BgcUser, long> manager, BgcUser user)
            => Task.Run(()
            => Notify(token, manager, user));

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<BgcUser, long> manager, BgcUser user)
            => Task.Run(()
            => Validate(purpose, token, manager, user));
    }
}
