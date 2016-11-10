using BGC.Utilities;
using CodeShield;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public partial class BgcUserTokenProvider : IUserTokenProvider<BgcUser, long>
    {
        private static readonly string[] ValidPurposes = (from field in typeof(TokenPurposes).GetFields(BindingFlags.Static | BindingFlags.Public)
                                                          where field.FieldType == typeof(string)
                                                          select field.GetValue(null) as string).ToArray();

        private static void VerifyPurposeIsValid(string purpose)
        {
            Shield.AssertOperation(purpose, p => ValidPurposes.Contains(purpose), $"The purpose '{purpose}' is not supported.");
        }

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
                    long validity = DateTime.UtcNow.AddDays(1).Ticks;
                    long userId = user.Id;
                    writer.Write(validity);
                    writer.Write(userId);
                    writer.Write(user.PasswordHash);
                    byte[] result = (writer.BaseStream as MemoryStream).ToArray();
                    System.Diagnostics.Trace.WriteLine($"Generated token: {result.ToStringAggregate(b => b.ToString("X2"))} or {result.ToBase62()}");
                    return result.ToBase62();
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

            System.Diagnostics.Trace.WriteLine($"Validating token: {token}");

            if (purpose == TokenPurposes.ResetPassword)
            {
               using (var reader = new BinaryReader(new MemoryStream(token.FromBase62())))
                {
                    long ticks = reader.ReadInt64();
                    DateTime validity = new DateTime(ticks);
                    long userId = reader.ReadInt64();
                    string passwordHash = reader.ReadString();
                    return
                        DateTime.UtcNow < validity &&
                        userId == user.Id &&
                        passwordHash == user.PasswordHash;
                }
            }

            return false; // we won't make it to this line here - if the purpose is invalid, an exception will be thrown during argument validation
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
