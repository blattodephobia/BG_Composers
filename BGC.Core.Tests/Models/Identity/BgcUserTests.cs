using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Core.Tests.Models.Identity.BgcUserTests
{
    [TestFixture]
    public class CheckPasswordResetToken
    {
        [Test]
        public void MatchesCorrectTokens()
        {
            BgcUser user = new BgcUser("Alice");
            user.SetPasswordResetTokenHash("ABCDEF");
            Assert.IsTrue(user.CheckPasswordResetToken("ABCDEF"));
        }

        public void DoesntMatchWrongTokens()
        {
            BgcUser user = new BgcUser("Alice");
            user.SetPasswordResetTokenHash("ABCDEF");
            Assert.IsFalse(user.CheckPasswordResetToken("123456"));
        }

        public void DoesntMatchWhenNoStoredHash()
        {
            BgcUser user = new BgcUser("Alice");
            Assert.IsFalse(user.CheckPasswordResetToken("123456"));
        }

        public void ThrowsExceptionIfInvalidToken_1()
        {
            BgcUser user = new BgcUser("Alice");
            Assert.Throws<InvalidOperationException>(() => user.CheckPasswordResetToken(null));
        }

        public void ThrowsExceptionIfInvalidToken_2()
        {
            BgcUser user = new BgcUser("Alice");
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
    
    public class AddSettingTests : TestFixtureBase
    {
        [Test]
        public void SetsOwnerStamp()
        {
            var user = new BgcUser("user");

            Setting setting = new Setting("name");
            user.AddSetting(setting);

            Assert.AreEqual(user.UserName, setting.OwnerStamp);
        }
    }

    [TestFixture]
    public class CtorTests
    {
        [Test]
        public void ThrowsExceptionIfNullString()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var user = new BgcUser(null);
            });
        }

        [Test]
        public void ThrowsExceptionIfEmptyString()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var user = new BgcUser("");
            });
        }
    }

    [TestFixture]
    public class UserNameTests
    {
        [Test]
        public void ThrowsExceptionIfNullString()
        {
            var user = new BgcUser("alice");
            Assert.Throws<InvalidOperationException>(() =>
            {
                user.UserName = null;
            });
        }

        [Test]
        public void ThrowsExceptionIfEmptyString()
        {
            var user = new BgcUser("alice");
            Assert.Throws<InvalidOperationException>(() =>
            {
                user.UserName = "";
            });
        }
    }

    [TestFixture]
    public class SetPasswordResetTokenHashTests
    {
        [Test]
        public void ComputesHashWithNonEmptyToken()
        {
            BgcUser user = new BgcUser("Alice");
            user.SetPasswordResetTokenHash("sflkjd");

            Assert.IsNotNull(user.PasswordResetTokenHash);
            Assert.IsNotEmpty(user.PasswordResetTokenHash);
        }

        [Test]
        public void ComputesHashWithEmptyToken()
        {
            BgcUser user = new BgcUser("Alice");
            user.SetPasswordResetTokenHash(null);

            Assert.IsNull(user.PasswordResetTokenHash);
        }
    }
}
