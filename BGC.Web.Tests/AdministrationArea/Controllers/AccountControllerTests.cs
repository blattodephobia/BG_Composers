using BGC.Core;
using BGC.Utilities;
using BGC.Web.Areas.Administration.Controllers;
using BGC.Web.Areas.Administration.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BGC.Web.Tests.AdministrationArea.Controllers
{
    [TestFixture]
    public class ResetPasswordTests
    {
        private static Mock<IUserStore<BgcUser, long>> GetMockUserStore(BgcUser mockUser, Mock chainMock = null)
        {
            var mockStore = chainMock?.As<IUserStore<BgcUser, long>>() ?? new Mock<IUserStore<BgcUser, long>>();
            mockStore
                .Setup(store => store.FindByIdAsync(mockUser.Id))
                .ReturnsAsync(mockUser);

            mockStore
                .Setup(store => store.FindByNameAsync(mockUser.UserName))
                .ReturnsAsync(mockUser);

            return mockStore;
        }

        private static Mock<IUserPasswordStore<BgcUser, long>> GetMockPasswordStore(BgcUser mockUser, Mock chainMock = null)
        {
            var mockStore = chainMock?.As<IUserPasswordStore<BgcUser, long>>() ?? new Mock<IUserPasswordStore<BgcUser, long>>();
            mockStore
                .Setup(store => store.SetPasswordHashAsync(It.IsAny<BgcUser>(), It.IsAny<string>()))
                .Callback((BgcUser u, string hash) => u.PasswordHash = hash)
                .Returns(Task.CompletedTask);

            mockStore
                .Setup(store => store.FindByNameAsync(It.Is<string>(s => s == mockUser.UserName)))
                .ReturnsAsync(mockUser);

            mockStore
                .Setup(store => store.FindByIdAsync(It.Is<long>(id => id == mockUser.Id)))
                .ReturnsAsync(mockUser);

            return mockStore;
        }

        private static Mock<IIdentityMessageService> GetMockEmailService(Action<IdentityMessage> callback)
        {
            var mockEmailService = new Mock<IIdentityMessageService>();
            mockEmailService
                .Setup(s => s.SendAsync(It.IsAny<IdentityMessage>()))
                .Callback(callback)
                .Returns(Task.CompletedTask);

            return mockEmailService;
        }

        private static Mock<IUserTokenProvider<BgcUser, long>> GetMockTokenProvider(string defaultPasswordResetToken, BgcUser testUser)
        {
            var mockTokenProvider = new Mock<IUserTokenProvider<BgcUser, long>>();
            mockTokenProvider
                .Setup(t => t.GenerateAsync(
                    It.Is<string>(s => s == TokenPurposes.ResetPassword),
                    It.IsAny<UserManager<BgcUser, long>>(),
                    It.IsAny<BgcUser>()))
                .ReturnsAsync(defaultPasswordResetToken);

            mockTokenProvider
                .Setup(t => t.ValidateAsync(
                    It.Is<string>(s => s == TokenPurposes.ResetPassword),
                    It.Is<string>(s => s == defaultPasswordResetToken),
                    It.IsAny<UserManager<BgcUser, long>>(),
                    It.Is<BgcUser>(u => u == testUser)))
                .ReturnsAsync(true);

            return mockTokenProvider;
        }

        private static Mock<SignInManager<BgcUser, long>> GetMockSignInManager(BgcUserManager mockManager, IAuthenticationManager authManager)
        {
            return new Mock<SignInManager<BgcUser, long>>(mockManager, authManager);
        }

        private static Mock<BgcUserManager> GetMockUserManager(BgcUser user, IUserStore<BgcUser, long> mockStore, IUserTokenProvider<BgcUser, long> mockTokenProvider)
        {
            Mock<BgcUserManager> mockManager = new Mock<BgcUserManager>(mockStore) { CallBase = true };
            mockManager
                .Setup(um => um.FindByEmailAsync(It.Is<string>(email => email == user.Email)))
                .ReturnsAsync(user);

            mockManager.Object.UserTokenProvider = mockTokenProvider;

            return mockManager;
        }

        [Test]
        public void ReturnsCorrectViewModel()
        {
            var user = new BgcUser() { Id = 5, PasswordHash = "old", Email = "sample_mail@host.com", UserName = "user" };
            string standardToken = "token";
            string sentEmail = "";

            #region Setup mocks
            Mock<IUserStore<BgcUser, long>> mockStore = GetMockUserStore(user);
            Mock<IUserTokenProvider<BgcUser, long>> mockTokenProvider = GetMockTokenProvider(standardToken, user);
            Mock<BgcUserManager> mockManager = GetMockUserManager(user, mockStore.Object, mockTokenProvider.Object);
            Mock<SignInManager<BgcUser, long>> sm = GetMockSignInManager(mockManager.Object, new Mock<IAuthenticationManager>().Object);
            Mock<IIdentityMessageService> mService = GetMockEmailService(message => sentEmail = message.Body);
            mockManager.Object.EmailService = mService.Object;
            #endregion

            AccountController ctrl = new AccountController(sm.Object)
            {
                UserManager = mockManager.Object,
                EncryptionKey = "123"
            };

            string encryptedEmail = Encoding.Unicode.GetBytes(user.Email).Encrypt(ctrl.EncryptionKey).ToBase62();
            var model = ((ctrl.ResetPassword(encryptedEmail, standardToken).Result as ViewResult).Model as PasswordResetViewModel);
            Assert.AreEqual(user.Email, model.Email);
            Assert.AreEqual(standardToken, model.Token);
        }

        [Test]
        public void ResetPasswordOnPost()
        {
            var user = new BgcUser() { Id = 5, PasswordHash = "old", Email = "sample_mail@host.com", UserName = "user" };
            string standardToken = "token";

            #region Setup mocks
            Mock<IUserStore<BgcUser, long>> mockStore = GetMockUserStore(user, GetMockPasswordStore(user));
            Mock<IUserTokenProvider<BgcUser, long>> mockTokenProvider = GetMockTokenProvider(standardToken, user);
            Mock<BgcUserManager> mockManager = GetMockUserManager(user, mockStore.Object, mockTokenProvider.Object);
            Mock<SignInManager<BgcUser, long>> sm = GetMockSignInManager(mockManager.Object, new Mock<IAuthenticationManager>().Object);
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.SetupAllProperties();
            #endregion

            AccountController ctrl = new AccountController(sm.Object) { UserManager = mockManager.Object };
            sm
                .Setup(x => x.SignInAsync(It.IsAny<BgcUser>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Callback((BgcUser u, bool a, bool b) =>
                {
                    var mockIdentity = new Mock<IIdentity>();
                    mockIdentity
                        .SetupGet(x => x.IsAuthenticated)
                        .Returns(true);
                    mockIdentity
                        .Setup(x => x.Name)
                        .Returns(u.UserName);

                    var mockUser = new Mock<IPrincipal>();
                    mockUser
                        .SetupGet(x => x.Identity)
                        .Returns(mockIdentity.Object);

                    mockHttpContext.Object.User = mockUser.Object;
                    ctrl.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), ctrl);
                })
                .Returns(Task.CompletedTask);
            ctrl.ResetPassword_Post(new PasswordResetViewModel() { Email = user.Email, Token = standardToken, NewPassword = "newnew", ConfirmPassword = "new" }).Wait();

            Assert.AreNotEqual("old", user.PasswordHash);
        }
    }
}
