using BGC.Core;
using BGC.Core.Models;
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
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
                .Setup(store => store.FindByIdAsync(It.Is<long>(u => u == mockUser.Id)))
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

        public static Mock<ISettingsService> GetMockSettingsService(IList<Setting> settingsRepo = null)
        {
            var mockService = new Mock<MockSettingsService>(settingsRepo);
            mockService.Setup(x => x.ReadSetting(It.IsNotNull<string>())).Returns((string name) => settingsRepo?.Where(s => s.Name == name).OrderByDescending(s => s.Priority).FirstOrDefault());
            return mockService.As<ISettingsService>();
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

        public static Mock<IPrincipal> GetMockUser(string name)
        {
            var identity = new Mock<IIdentity>();
            identity.SetupGet(x => x.Name).Returns(name);
            identity.SetupGet(x => x.IsAuthenticated).Returns(true);

            var result = new Mock<IPrincipal>();
            result.SetupGet(x => x.Identity).Returns(identity.Object);
            return result;
        }

        public static Mock<HttpResponseBase> GetMockResponseBase(MockBehavior behavior = MockBehavior.Loose)
        {
            var result = new Mock<HttpResponseBase>(behavior);

            return result;
        }

        public static Mock<RequestContext> GetMockRequestContext(HttpRequestBase request = null, HttpResponseBase response = null)
        {
            var result = new Mock<RequestContext>();
            result.SetupGet(r => r.HttpContext).Returns(GetMockHttpContextBase(request, response).Object);

            return result;
        }

        public static Mock<HttpContextBase> GetMockHttpContextBase(HttpRequestBase request = null, HttpResponseBase response = null)
        {
            var result = new Mock<HttpContextBase>();
            result.SetupGet(x => x.Request).Returns(request);
            result.SetupGet(x => x.Response).Returns(response);

            return result;
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
                .Setup(s => s.AddOrUpdate(It.IsAny<Composer>()))
                .Callback((Composer c) => composers.Add(c));

            mockService
                .Setup(s => s.FindComposer(It.IsAny<Guid>()))
                .Returns((Guid id) => composers.FirstOrDefault(c => c.Id == id));

            return mockService;
        }

        /// <summary>
        /// Returns a mock of the <see cref="IArticleContentService"/>. Make sure the <see cref="ComposerArticle"/> instances override <see cref="object.ToString()"/>,
        /// if comparing their text output will be required.
        /// </summary>
        /// <param name="articles"></param>
        /// <returns></returns>
        public static Mock<IArticleContentService> GetMockArticleService(IDictionary<Guid, string> articles)
        {
            Mock<IArticleContentService> mockService = new Mock<IArticleContentService>();
            mockService
                .Setup(s => s.GetEntry(It.IsAny<Guid>()))
                .Returns((Guid id) => articles.ContainsKey(id) ? articles[id] : null);

            mockService
                .Setup(s => s.RemoveEntry(It.IsAny<Guid>()))
                .Callback((Guid id) => articles.Remove(id));

            mockService
                .Setup(s => s.StoreEntry(It.IsAny<string>()))
                .Returns((string s) =>
                {
                    var id = Guid.NewGuid();
                    articles.Add(id, s);
                    return id;
                });

            mockService
                .Setup(s => s.UpdateEntry(It.IsAny<Guid>(), It.IsAny<string>()))
                .Callback((Guid id, string s) => articles[id] = s);

            return mockService;
        }

        public static Mock<IGlossaryService> GetMockGlossaryService(List<GlossaryEntry> backingStore)
        {
            var mockService = new Mock<IGlossaryService>();
            mockService
                .Setup(s => s.ListAll())
                .Returns(() => backingStore);

            mockService
                .Setup(s => s.Delete(It.IsAny<GlossaryEntry>()))
                .Callback((GlossaryEntry entry) => backingStore.Remove(entry));

            mockService
                .Setup(s => s.Find(It.IsAny<Guid>()))
                .Returns((Guid id) => backingStore.FirstOrDefault(x => x?.Id == id));

            mockService
                .Setup(s => s.AddOrUpdate(It.IsAny<GlossaryEntry>()))
                .Callback((GlossaryEntry entry) => backingStore.Add(entry));

            return mockService;
        }

        public static T GetActionResultModel<T>(this ActionResult result) where T : class => (result as ViewResultBase).Model as T;

        public static Mock<HttpRequestBase> GetMockRequestBase(MockBehavior behavior = default(MockBehavior))
        {
            return new Mock<HttpRequestBase>(behavior);
        }

        public static Mock<HttpContextBase> GetMockContext(MockBehavior behavior = default(MockBehavior))
        {
            return new Mock<HttpContextBase>(behavior);
        }

        public static Mock<IGeoLocationService> GetMockGeoLocationService(Dictionary<IPAddress, IEnumerable<CultureInfo>> db = null)
        {
            var mockService = new Mock<IGeoLocationService>();
            if (db != null)
            {
                mockService.Setup(x => x.GetCountry(It.IsNotNull<IPAddress>())).Returns((IPAddress ip) => db.ContainsKey(ip) ? new CountryInfo(db[ip].Select(c => c.Name.Substring(2, 2)).First()) : null);
            }
            return mockService;
        }

        public static Mock<ISearchService> GetMockComposerSearchService(IList<Composer> composers)
        {
            var mockService = new Mock<ISearchService>();
            mockService.Setup(s => s.Search(It.IsAny<string>())).Returns<string>(q => composers?.Where(c => c.LocalizedNames.Any(name => name.FullName.Contains(q))).Select(name => new SearchResult() { Header = name.Id.ToString() }));

            return mockService;
        }

        public static ApplicationProfile GetStandardAppProfile()
        {
            return new ApplicationProfile()
            {
                LocaleCookieName = "localeCookie",
                LocaleRouteTokenName = "locale",
                LocaleKey = "locale",
                SupportedLanguages = new List<CultureInfo>() { CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("de-DE") }
            };
        }
    }
}
