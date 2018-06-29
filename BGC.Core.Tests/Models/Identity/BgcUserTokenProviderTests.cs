using BGC.Data;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestUtils;

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
            var mockRoleRepo = new Mock<IRepository<BgcRole>>();
            var mockInvitationRepo = new Mock<IRepository<Invitation>>();
            var um = new BgcUserManager(mockStore.Object, mockRoleRepo.Object, mockInvitationRepo.Object);
            var user = new BgcUser("Alice") { Id = 5, Email = "email", PasswordHash = "abcdef" };
            mockStore.Setup(m => m.FindByIdAsync(user.Id)).Returns(Task.Run(() => user));

            BgcUserTokenProvider provider = new BgcUserTokenProvider();
            Assert.IsTrue(provider.IsValidProviderForUser(um, user));
        }

        [Test]
        public void RejectsInvalidUsers_Missing()
        {
            var mockStore = new Mock<IUserStore<BgcUser, long>>();
            var mockRoleRepo = new Mock<IRepository<BgcRole>>();
            var mockInvitationRepo = new Mock<IRepository<Invitation>>();
            var um = new BgcUserManager(mockStore.Object, mockRoleRepo.Object, mockInvitationRepo.Object);
            var user = new BgcUser("Alice") { Email = "email", PasswordHash = "abcdef" };
            mockStore.Setup(m => m.FindByIdAsync(user.Id)).Returns(Task.Run(() => (BgcUser)null));

            BgcUserTokenProvider provider = new BgcUserTokenProvider();
            Assert.IsFalse(provider.IsValidProviderForUser(um, user));
        }

        [Test]
        public void RejectsInvalidUsers_NoEmail()
        {
            var mockStore = new Mock<IUserStore<BgcUser, long>>();
            var mockRoleRepo = new Mock<IRepository<BgcRole>>();
            var mockInvitationRepo = new Mock<IRepository<Invitation>>();
            var um = new BgcUserManager(mockStore.Object, mockRoleRepo.Object, mockInvitationRepo.Object);
            var user = new BgcUser("Alice") { Id = 5, PasswordHash = "abcdef" };
            mockStore.Setup(m => m.FindByIdAsync(user.Id)).Returns(Task.Run(() => user));

            BgcUserTokenProvider provider = new BgcUserTokenProvider();
            Assert.IsFalse(provider.IsValidProviderForUser(um, user));
        }

        [Test]
        public void RejectsInvalidUsers_NoPassword()
        {
            var mockStore = new Mock<IUserStore<BgcUser, long>>();
            var mockRoleRepo = new Mock<IRepository<BgcRole>>();
            var mockInvitationRepo = new Mock<IRepository<Invitation>>();
            var um = new BgcUserManager(mockStore.Object, mockRoleRepo.Object, mockInvitationRepo.Object);
            var user = new BgcUser("Alice") { Id = 5, Email = "email" };
            mockStore.Setup(m => m.FindByIdAsync(user.Id)).Returns(Task.Run(() => user));

            BgcUserTokenProvider provider = new BgcUserTokenProvider();
            Assert.IsFalse(provider.IsValidProviderForUser(um, user));
        }
    }

    [TestFixture]
    public class ValidateTests
    {
        [Test]
        public void RejectsTokensAfterPasswordHasChanged()
        {
            var mockStore = new Mock<IUserStore<BgcUser, long>>();
            var mockRoleRepo = new Mock<IRepository<BgcRole>>();
            var mockInvitationRepo = new Mock<IRepository<Invitation>>();
            var um = new BgcUserManager(mockStore.Object, mockRoleRepo.Object, mockInvitationRepo.Object);
            var user = new BgcUser("Alice") { Id = 5, Email = "email", PasswordHash = "old" };
            mockStore.Setup(m => m.FindByIdAsync(user.Id)).Returns(Task.Run(() => user));

            BgcUserTokenProvider provider = new BgcUserTokenProvider();
            string oldToken = provider.Generate(TokenPurposes.ResetPassword, um, user);
            Assert.IsTrue(provider.Validate(TokenPurposes.ResetPassword, oldToken, um, user));

            user.PasswordHash = "new";

            Assert.IsFalse(provider.Validate(TokenPurposes.ResetPassword, oldToken, um, user));
        }

        [Test]
        public void RejectsExpiredTokens()
        {
            var mockStore = new Mock<IUserStore<BgcUser, long>>();
            var mockRoleRepo = new Mock<IRepository<BgcRole>>();
            var mockInvitationRepo = new Mock<IRepository<Invitation>>();
            var um = new BgcUserManager(mockStore.Object, mockRoleRepo.Object, mockInvitationRepo.Object);
            var user = new BgcUser("Alice") { Id = 5, Email = "email", PasswordHash = "old" };
            mockStore.Setup(m => m.FindByIdAsync(user.Id)).Returns(Task.Run(() => user));

            BgcUserTokenProvider provider = new BgcUserTokenProvider() { TokenExpiration = TimeSpan.FromMilliseconds(10) };
            string token = provider.Generate(TokenPurposes.ResetPassword, um, user);
            Thread.Sleep(provider.TokenExpiration);
            Assert.IsFalse(provider.Validate(TokenPurposes.ResetPassword, token, um, user));
        }

        [Test]
        public void ValidatesOwnUsersToken()
        {
            var mockStore = new Mock<IUserStore<BgcUser, long>>();
            var mockRoleRepo = new Mock<IRepository<BgcRole>>();
            var mockInvitationRepo = new Mock<IRepository<Invitation>>();
            var um = new BgcUserManager(mockStore.Object, mockRoleRepo.Object, mockInvitationRepo.Object);
            var user1 = new BgcUser("Alice") { Id = 5, Email = "email1", PasswordHash = "old" };
            var user2 = new BgcUser("Alice") { Id = 5, Email = "email2", PasswordHash = "old" };
            mockStore.Setup(m => m.FindByIdAsync(user1.Id)).Returns(Task.Run(() => user1));
            mockStore.Setup(m => m.FindByIdAsync(user2.Id)).Returns(Task.Run(() => user2));

            BgcUserTokenProvider provider = new BgcUserTokenProvider() { TokenExpiration = TimeSpan.FromDays(10) };
            string token = provider.Generate(TokenPurposes.ResetPassword, um, user1);

            Assert.IsFalse(provider.Validate(TokenPurposes.ResetPassword, token, um, user2));
        }
    }
    
    public class TokenGenerationTests : TestFixtureBase
    {
        private BgcUserTokenProvider _provider;
        private Mock<IUserStore<BgcUser, long>> _mockStore;
        private Mock<IRepository<BgcRole>> _mockRoleRepo;
        private Mock<IRepository<Invitation>> _mockInvitationRepo;
        private BgcUserManager _um;
        private BgcUser _user;

        public override void OneTimeSetUp()
        {
            _user = new BgcUser("Alice") { Email = "email", PasswordHash = "ABCDEF" };
            _mockStore = MockUtilities.GetMockUserStore(_user);
            _provider = new BgcUserTokenProvider();
            _mockRoleRepo = new Mock<IRepository<BgcRole>>();
            _mockInvitationRepo = new Mock<IRepository<Invitation>>();
            _um = new BgcUserManager(_mockStore.Object, _mockRoleRepo.Object, _mockInvitationRepo.Object);
        }

        [Test]
        public void GeneratesValidTokens()
        {
            string token = _provider.Generate(TokenPurposes.ResetPassword, _um, _user);
            Assert.IsTrue(_provider.Validate(TokenPurposes.ResetPassword, token, _um, _user));
        }

        [Test]
        public void DoesntGenerateTokensIfUserIsNotPresent()
        {
            // the provider should call IsValidProviderForUser before operations
            Assert.Throws<InvalidOperationException>(() =>
            {
                _provider.Generate(TokenPurposes.ResetPassword, _um, new BgcUser("Alice") { Email = "test" }); // user has email, but doesn't exist in the IUserStore
            });
        }

        [Test]
        public void DoesntGenerateTokensIfUserHasNoEmail()
        {
            // the provider should call IsValidProviderForUser before operations
            _user = new BgcUser("Alice") { Id = 5 };
            Assert.Throws<InvalidOperationException>(() => _provider.Generate(TokenPurposes.ResetPassword, _um, _user)); // user has no email; the provider is not valid
        }

        [Test]
        public void GeneratesDifferentTokensForSameUser()
        {
            _user = new BgcUser("Alice") { Email = "email", PasswordHash = "ABCDEF" };

            string token1 = _provider.Generate(TokenPurposes.ResetPassword, _um, _user);
            string token2 = _provider.Generate(TokenPurposes.ResetPassword, _um, _user);

            Assert.AreNotEqual(token1, token2);
        }
    }
}
