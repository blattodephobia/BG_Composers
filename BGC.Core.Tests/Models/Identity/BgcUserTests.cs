using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models.Identity
{
    [TestFixture]
    public class CheckPasswordResetToken
    {
        [Test]
        public void MatchesCorrectTokens()
        {
            BgcUser user = new BgcUser();
            user.SetPasswordResetTokenHash("ABCDEF");
            Assert.IsTrue(user.CheckPasswordResetToken("ABCDEF"));
        }
        
        public void DoesntMatchWrongTokens()
        {
            BgcUser user = new BgcUser();
            user.SetPasswordResetTokenHash("ABCDEF");
            Assert.IsFalse(user.CheckPasswordResetToken("123456"));
        }
        
        public void DoesntMatchWhenNoStoredHash()
        {
            BgcUser user = new BgcUser();
            Assert.IsFalse(user.CheckPasswordResetToken("123456"));
        }
        
        public void ThrowsExceptionIfInvalidToken_1()
        {
            BgcUser user = new BgcUser();
            Assert.Throws<InvalidOperationException>(() => user.CheckPasswordResetToken(null));
        }
        
        public void ThrowsExceptionIfInvalidToken_2()
        {
            BgcUser user = new BgcUser();
            Assert.Throws<InvalidOperationException>(() => user.CheckPasswordResetToken(""));
        }
    }

    [TestFixture]
    public class PasswordResetTokenHashTests
    {
        [Test]
        public void MaxLengthMatchesGeneratedBase64String()
        {
            string base64String = Convert.ToBase64String(new byte[BgcUser.PASSWORD_RESET_TOKEN_LENGTH]);
            StringLengthAttribute strLengthAttr = typeof(BgcUser).GetProperty(nameof(BgcUser.PasswordResetTokenHash)).GetCustomAttribute<StringLengthAttribute>();
            Assert.AreEqual(base64String.Length, strLengthAttr.MaximumLength);
        }
    }
}
