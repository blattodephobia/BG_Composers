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
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using static TestUtils.Mocks;

namespace BGC.Web.Tests.AdministrationArea.Controllers
{
    [TestFixture]
    public class ResetPasswordTests
    {

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
