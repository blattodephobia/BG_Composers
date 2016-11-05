using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests
{
    [TestFixture]
    public class IsValidProviderTests
    {
        [Test]
        public void AcceptsValidUsers()
        {
            // valid users are present in the IUserStore, have an Email and a set password
            var mockStore = new Mock<IUserStore<BgcUser, long>>();
            var um = new BgcUserManager(mockStore.Object);
            BgcUserTokenProvider provider = new BgcUserTokenProvider();
            var user = new BgcUser() { Id = 5, Email = "email", PasswordHash = "abcdef" };
            mockStore.Setup(m => m.FindByIdAsync(user.Id)).Returns(Task.Run(() => user));
            Assert.IsTrue(provider.IsValidProviderForUser(um, user));
        }

        [Test]
        public void RejectsInvalidUsers_Missing()
        {
            var mockStore = new Mock<IUserStore<BgcUser, long>>();
            var um = new BgcUserManager(mockStore.Object);
            BgcUserTokenProvider provider = new BgcUserTokenProvider();
            var user = new BgcUser() { Email = "email", PasswordHash = "abcdef" };
            mockStore.Setup(m => m.FindByIdAsync(user.Id)).Returns(Task.Run(() => (BgcUser)null));
            Assert.IsFalse(provider.IsValidProviderForUser(um, user));
        }

        [Test]
        public void RejectsInvalidUsers_NoEmail()
        {
            var mockStore = new Mock<IUserStore<BgcUser, long>>();
            var um = new BgcUserManager(mockStore.Object);
            BgcUserTokenProvider provider = new BgcUserTokenProvider();
            var user = new BgcUser() { Id = 5, PasswordHash = "abcdef" };
            mockStore.Setup(m => m.FindByIdAsync(user.Id)).Returns(Task.Run(() => user));
            Assert.IsFalse(provider.IsValidProviderForUser(um, user));
        }

        [Test]
        public void RejectsInvalidUsers_NoPassword()
        {
            var mockStore = new Mock<IUserStore<BgcUser, long>>();
            var um = new BgcUserManager(mockStore.Object);
            BgcUserTokenProvider provider = new BgcUserTokenProvider();
            var user = new BgcUser() { Id = 5, Email = "email" };
            mockStore.Setup(m => m.FindByIdAsync(user.Id)).Returns(Task.Run(() => user));
            Assert.IsFalse(provider.IsValidProviderForUser(um, user));
        }
    }

    [TestFixture]
    public class TokenGenerationTests
    {
        [Test]
        public void GeneratesValidTokens()
        {
            BgcUserTokenProvider provider = new BgcUserTokenProvider();
            var mockStore = new Mock<IUserStore<BgcUser, long>>();
            var um = new BgcUserManager(mockStore.Object);
            var user = new BgcUser() { Email = "email", PasswordHash = "ABCDEF" };
            mockStore.Setup(m => m.FindByIdAsync(user.Id)).Returns(Task.Run(() => user));
            string token = provider.Generate(BgcUserTokenProvider.Purposes.PasswordReset, um, user);
            Assert.IsTrue(provider.Validate(BgcUserTokenProvider.Purposes.PasswordReset, token, um, user));
        }

        [Test]
        public void DoesntGenerateTokensIfUserIsNotPresent()
        {
            // the provider should call IsValidProviderForUser before operations
            BgcUserTokenProvider provider = new BgcUserTokenProvider();
            var mockStore = new Mock<IUserStore<BgcUser, long>>();
            var um = new BgcUserManager(mockStore.Object);
            Assert.Throws<InvalidOperationException>(() => provider.Generate(BgcUserTokenProvider.Purposes.PasswordReset, um, new BgcUser() { Email = "test" })); // user has email, but doesn't exist in the IUserStore
        }

        [Test]
        public void DoesntGenerateTokensIfUserHasNoEmail()
        {
            // the provider should call IsValidProviderForUser before operations
            BgcUserTokenProvider provider = new BgcUserTokenProvider();
            var mockStore = new Mock<IUserStore<BgcUser, long>>();
            var um = new BgcUserManager(mockStore.Object);
            var user = new BgcUser() { Id = 5 };
            mockStore.Setup(m => m.FindByIdAsync(user.Id)).Returns(Task.Run(() => user));
            Assert.Throws<InvalidOperationException>(() => provider.Generate(BgcUserTokenProvider.Purposes.PasswordReset, um, user)); // user has no email; the provider is not valid
        }
    }
}
