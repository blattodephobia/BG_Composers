﻿using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models.Identity
{
    [TestFixture]
    public class BgcUserManagerTests
    {
        public class BgcUserManagerProxy : BgcUserManager
        {
            public BgcUserManagerProxy(IUserStore<BgcUser, long> store, IRepository<BgcRole> roleRepo, IRepository<Invitation> invitationRepo) :
                base(store, roleRepo, invitationRepo)
            {
            }

            public int UserUpdatesCalled { get; set; }

            public event Action<BgcUser, string> UpdatePasswordCallback;

            public override Task<IdentityResult> UpdateAsync(BgcUser user) => Task.Run(() =>
            {
                UserUpdatesCalled++;
                return new IdentityResult();
            });

            protected override Task<IdentityResult> UpdatePassword(IUserPasswordStore<BgcUser, long> passwordStore, BgcUser user, string newPassword) => Task.Run(() =>
            {
                UpdatePasswordCallback?.Invoke(user, newPassword);
                return IdentityResult.Success;
            });
        }

        [Test]
        public void PasswordResetTokenGenerationTest()
        {
            Mock<IUserTokenProvider<BgcUser, long>> mockTokenProvider = new Mock<IUserTokenProvider<BgcUser, long>>();
            mockTokenProvider.Setup(t => t.GenerateAsync(It.IsAny<string>(), It.IsAny<UserManager<BgcUser, long>>(), It.IsAny<BgcUser>())).ReturnsAsync("token");

            Mock<IUserStore<BgcUser, long>> mockStore = new Mock<IUserStore<BgcUser, long>>();
            var user = new BgcUser() { Id = 5 };
            mockStore.Setup(store => store.FindByIdAsync(user.Id)).ReturnsAsync(user);

            var mockRoleRepo = new Mock<IRepository<BgcRole>>();
            var mockInvitationRepo = new Mock<IRepository<Invitation>>();
            var manager = new BgcUserManagerProxy(mockStore.Object, mockRoleRepo.Object, mockInvitationRepo.Object) { UserTokenProvider = mockTokenProvider.Object, };
            manager.GeneratePasswordResetToken(user.Id);
            Assert.IsNotNull(user.PasswordResetTokenHash);
            Assert.AreEqual(1, manager.UserUpdatesCalled);
        }

        [Test]
        public void InvalidPasswordResetToken_FailsHashCheck()
        {
            Mock<IUserTokenProvider<BgcUser, long>> mockTokenProvider = new Mock<IUserTokenProvider<BgcUser, long>>();
            mockTokenProvider.Setup(t => t.GenerateAsync(It.IsAny<string>(), It.IsAny<UserManager<BgcUser, long>>(), It.IsAny<BgcUser>())).ReturnsAsync("token");

            Mock<IUserStore<BgcUser, long>> mockStore = new Mock<IUserStore<BgcUser, long>>();
            var user = new BgcUser() { Id = 5 };
            mockStore.Setup(store => store.FindByIdAsync(user.Id)).ReturnsAsync(user);

            var mockRoleRepo = new Mock<IRepository<BgcRole>>();
            var mockInvitationRepo = new Mock<IRepository<Invitation>>();
            var manager = new BgcUserManagerProxy(mockStore.Object, mockRoleRepo.Object, mockInvitationRepo.Object) { UserTokenProvider = mockTokenProvider.Object, };
            string corruptToken = manager.GeneratePasswordResetToken(user.Id) + "1";
            Assert.IsFalse(manager.ResetPassword(user.Id, corruptToken, "test").Succeeded);
        }

        [Test]
        public void Integration_AcceptsEncryptedTokens()
        {
            var user = new BgcUser() { Id = 5, PasswordHash = "old", Email = "sample_mail@host.com", UserName = "user" };
            Mock<IUserStore<BgcUser, long>> mockStore = new Mock<IUserStore<BgcUser, long>>();
            mockStore.Setup(store => store.FindByIdAsync(user.Id)).ReturnsAsync(user);

            Mock<IUserPasswordStore<BgcUser, long>> asPwdStore = mockStore.As<IUserPasswordStore<BgcUser, long>>();
            asPwdStore.Setup(x => x.SetPasswordHashAsync(It.IsAny<BgcUser>(), It.IsAny<string>())).Callback((BgcUser u, string pass) =>
            {
                u.PasswordHash = pass;
            });

            var mockRoleRepo = new Mock<IRepository<BgcRole>>();
            var mockInvitationRepo = new Mock<IRepository<Invitation>>();
            var manager = new BgcUserManagerProxy(mockStore.Object, mockRoleRepo.Object, mockInvitationRepo.Object) { UserTokenProvider = new BgcUserTokenProvider(), };
            manager.UpdatePasswordCallback += (u, pass) => u.PasswordHash = pass;
            string encryptedToken = manager.GeneratePasswordResetToken(user.Id);
            manager.ResetPasswordAsync(user.Id, encryptedToken, "new").Wait();
            Assert.AreEqual("new", user.PasswordHash);
        }

        [Test]
        public void Integration_RejectsWrongEncryptedTokens()
        {
            var user = new BgcUser() { Id = 5, PasswordHash = "old", Email = "sample_mail@host.com", UserName = "user" };
            Mock<IUserStore<BgcUser, long>> mockStore = new Mock<IUserStore<BgcUser, long>>();
            mockStore.Setup(store => store.FindByIdAsync(user.Id)).ReturnsAsync(user);

            Mock<IUserPasswordStore<BgcUser, long>> asPwdStore = mockStore.As<IUserPasswordStore<BgcUser, long>>();
            asPwdStore.Setup(x => x.SetPasswordHashAsync(It.IsAny<BgcUser>(), It.IsAny<string>())).Callback((BgcUser u, string pass) =>
            {
                u.PasswordHash = pass;
            });

            var mockRoleRepo = new Mock<IRepository<BgcRole>>();
            var mockInvitationRepo = new Mock<IRepository<Invitation>>();
            var manager = new BgcUserManagerProxy(mockStore.Object, mockRoleRepo.Object, mockInvitationRepo.Object) { UserTokenProvider = new BgcUserTokenProvider(), };
            manager.UpdatePasswordCallback += (u, pass) => u.PasswordHash = pass;
            string encryptedToken = manager.GeneratePasswordResetToken(user.Id);
            IdentityResult resetResult = manager.ResetPasswordAsync(user.Id, encryptedToken + "corrupt", "new").Result;
            Assert.IsFalse(resetResult.Succeeded);
        }
    }
}
