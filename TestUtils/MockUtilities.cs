using BGC.Core;
using BGC.Core.Services;
using BGC.Web.Models;
using BGC.Web.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace TestUtils
{
    public static class MockUtilities
    {
        static MockUtilities()
        {
            SampleLocalization = new XmlDocument();
            var assemblyFileName = new FileInfo(typeof(MockUtilities).Assembly.Location);
            SampleLocalization.Load(assemblyFileName.Directory.GetFiles(@"SampleLoc.xml").First().OpenRead());
        }

        public static readonly XmlDocument SampleLocalization;
        public static Mock<IUserStore<BgcUser, long>> GetMockUserStore(BgcUser mockUser, Mock chainMock = null)
        {
            var mockStore = chainMock?.As<IUserStore<BgcUser, long>>() ?? new Mock<IUserStore<BgcUser, long>>();
            mockStore
                .Setup(store => store.FindByIdAsync(It.Is<long>(u => u== mockUser.Id)))
                .ReturnsAsync(mockUser);

            mockStore
                .Setup(store => store.FindByNameAsync(It.Is<string>(s => s == mockUser.UserName)))
                .ReturnsAsync(mockUser);

            return mockStore;
        }

        public static Mock<IUserEmailStore<BgcUser, long>> GetMockEmailStore(List<BgcUser> users, Mock chainMock = null)
        {
            var mockStore = chainMock?.As<IUserEmailStore<BgcUser, long>>() ?? new Mock<IUserEmailStore<BgcUser, long>>();
            mockStore
                .Setup(store => store.FindByEmailAsync(It.IsAny<string>()))
                .Returns((string email) => Task.Run(() => users.FirstOrDefault(u => u.Email == email)));

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

        public static Mock<IComposerDataService> GetMockComposerService(IList<Composer> composers)
        {
            Mock<IComposerDataService> mockService = new Mock<IComposerDataService>();
            mockService
                .Setup(s => s.GetAllComposers())
                .Returns(composers);

            mockService
                .Setup(s => s.GetNames(It.IsAny<CultureInfo>()))
                .Returns((CultureInfo language) => composers.SelectMany(c => c.LocalizedNames).Where(name => name.Language == language).ToList());

            mockService
                .Setup(s => s.Add(It.IsAny<Composer>()))
                .Callback((Composer c) => composers.Add(c));

            return mockService;
        }

        /// <summary>
        /// Returns a mock of the <see cref="IArticleContentService"/>. Make sure the <see cref="ComposerArticle"/> instances override <see cref="object.ToString()"/>,
        /// if comparing their text output will be required.
        /// </summary>
        /// <param name="articles"></param>
        /// <returns></returns>
        public static Mock<IArticleContentService> GetMockArticleService(IList<ComposerArticle> articles)
        {
            Mock<IArticleContentService> mockService = new Mock<IArticleContentService>();
            mockService
                .Setup(s => s.GetEntry(It.IsAny<Guid>()))
                .Returns((Guid id) => articles.FirstOrDefault(a => a.StorageId == id)?.ToString());

            mockService
                .Setup(s => s.RemoveEntry(It.IsAny<Guid>()))
                .Callback((Guid id) => articles.Remove(articles.First(a => a.StorageId == id)));

            return mockService;
        }

        public static T GetActionResultModel<T>(this ActionResult result) where T : class => (result as ViewResultBase).Model as T;

        public static Mock<HttpRequestBase> GetMockRequest()
        {
            return new Mock<HttpRequestBase>();
        }

        public static Mock<HttpContextBase> GetMockContext()
        {
            return new Mock<HttpContextBase>();
        }

        public static Mock<IGeoLocationService> GetMockGeoLocationService(Dictionary<IPAddress, IEnumerable<CultureInfo>> db)
        {
            var mockService = new Mock<IGeoLocationService>();
            mockService.Setup(x => x.GetCountry(It.IsNotNull<IPAddress>())).Returns((IPAddress ip) => new CountryInfo(db[ip].Select(c => c.Name.Substring(2, 2)).First()));
            return mockService;
        }

        public static Mock<ISearchService> GetMockComposerSearchService(IList<Composer> composers)
        {
            var mockService = new Mock<ISearchService>();
            mockService.Setup(s => s.Search(It.IsAny<string>())).Returns<string>(q => composers?.Where(c => c.LocalizedNames.Any(name => name.FullName.Contains(q))).Select(name => new SearchResult() { Header = name.Id.ToString() }));

            return mockService;
        }
    }
}
