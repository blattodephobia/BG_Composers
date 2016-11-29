using BGC.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtils
{
    public static class Mocks
    {
        public static Mock<IUserStore<BgcUser, long>> GetMockUserStore(BgcUser mockUser, Mock chainMock = null)
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

        public static Mock<IUserPasswordStore<BgcUser, long>> GetMockPasswordStore(BgcUser mockUser, Mock chainMock = null)
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

        public static Mock<IIdentityMessageService> GetMockEmailService(Action<IdentityMessage> callback)
        {
            var mockEmailService = new Mock<IIdentityMessageService>();
            mockEmailService
                .Setup(s => s.SendAsync(It.IsAny<IdentityMessage>()))
                .Callback(callback)
                .Returns(Task.CompletedTask);

            return mockEmailService;
        }

        public static Mock<IUserTokenProvider<BgcUser, long>> GetMockTokenProvider(string defaultPasswordResetToken, BgcUser testUser)
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

        public static Mock<SignInManager<BgcUser, long>> GetMockSignInManager(BgcUserManager mockManager, IAuthenticationManager authManager)
        {
            return new Mock<SignInManager<BgcUser, long>>(mockManager, authManager);
        }

        public static Mock<IRepository<T>> GetMockRepository<T>(List<T> entities) where T : class
        {
            var mockRepo = new Mock<IRepository<T>>();
            mockRepo
                .Setup(m => m.All())
                .Returns(entities.AsQueryable());

            mockRepo
                .Setup(m => m.Delete(It.IsNotNull<T>()))
                .Callback((T entity) => entities.Remove(entity));

            mockRepo
                .Setup(m => m.Insert(It.IsNotNull<T>()))
                .Callback((T entity) => entities.Add(entity));

            mockRepo
                .SetupGet(x => x.UnitOfWork)
                .Returns(new Mock<IUnitOfWork>().Object);

            return mockRepo;
        }

        public static Mock<BgcUserManager> GetMockUserManager(BgcUser user, IUserStore<BgcUser, long> mockStore, IUserTokenProvider<BgcUser, long> mockTokenProvider, IRepository<BgcRole> mockRoleRepository = null, IRepository<Invitation> mockInvitationsRepo = null)
        {
            Mock<BgcUserManager> mockManager = new Mock<BgcUserManager>(mockStore, mockRoleRepository ?? GetMockRepository(new List<BgcRole>()).Object, mockInvitationsRepo ?? GetMockRepository(new List<Invitation>()).Object) { CallBase = true };
            mockManager
                .Setup(um => um.FindByEmailAsync(It.Is<string>(email => email == user.Email)))
                .ReturnsAsync(user);

            mockManager.Object.UserTokenProvider = mockTokenProvider;

            return mockManager;
        }
    }
}
