using BGC.Core;
using BGC.Core.Models;
using BGC.Core.Services;
using BGC.Data;
using BGC.Data.Relational;
using BGC.Data.Relational.Mappings;
using BGC.Web;
using BGC.Web.Models;
using BGC.Web.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
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

        public static readonly Func<Type, DbSet> DefaultSetFactory = (delegate (Type t)
        {
            return GetMockDbSet().Object;
        });

        public static readonly XmlDocument SampleLocalization;
        public static Mock<IUserStore<BgcUser, long>> GetMockUserStore(BgcUser mockUser = null, Mock chainMock = null)
        {
            var mockStore = chainMock?.As<IUserStore<BgcUser, long>>() ?? new Mock<IUserStore<BgcUser, long>>();
            if (mockUser != null)
            {
                mockStore
                .Setup(store => store.FindByIdAsync(It.Is<long>(u => u == mockUser.Id)))
                .ReturnsAsync(mockUser);

                mockStore
                    .Setup(store => store.FindByNameAsync(It.Is<string>(s => s == mockUser.UserName)))
                    .ReturnsAsync(mockUser);
            }

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

        public static Mock<INonQueryableRepository<TKey, TEntity>> GetMockNonQueryRepo<TKey, TEntity>(Dictionary<TKey, TEntity> backingStore)
            where TKey : struct
            where TEntity : BgcEntity<TKey>
        {
            var result = new Mock<INonQueryableRepository<TKey, TEntity>>();
            result.Setup(r => r.AddOrUpdate(It.IsAny<TEntity>())).Callback((TEntity entity) =>
            {
                if (backingStore.ContainsKey(entity.Id))
                {
                    backingStore[entity.Id] = entity;
                }
                else
                {
                    backingStore.Add(entity.Id, entity);
                }
            });

            result.Setup(r => r.Delete(It.IsAny<TKey[]>())).Callback((TKey[] keys) =>
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (backingStore.ContainsKey(keys[i])) backingStore.Remove(keys[i]);
                }
            });

            result.Setup(r => r.Find(It.IsAny<TKey>())).Returns((TKey key) =>
            {
                TEntity entity;
                backingStore.TryGetValue(key, out entity);
                return entity;
            });

            return result;
        }

        public static Mock<IUserPasswordStore<BgcUser, long>> GetMockPasswordStore(BgcUser mockUser, Mock chainMock = null)
        {
            var mockStore = chainMock?.As<IUserPasswordStore<BgcUser, long>>() ?? new Mock<IUserPasswordStore<BgcUser, long>>();
            
            mockStore
                .Setup(store => store.SetPasswordHashAsync(It.IsAny<BgcUser>(), It.IsAny<string>()))
                .Callback((BgcUser u, string hash) => u.PasswordHash = hash)
                .Returns(Task.CompletedTask);
            if (mockUser != null)
            {
                mockStore
                .Setup(store => store.FindByNameAsync(It.Is<string>(s => s == mockUser.UserName)))
                .ReturnsAsync(mockUser);

                mockStore
                    .Setup(store => store.FindByIdAsync(It.Is<long>(id => id == mockUser.Id)))
                    .ReturnsAsync(mockUser);
            }

            return mockStore;
        }

        public static Mock<IMediaService> GetMockMediaService(IEnumerable<MediaTypeInfo> mediaRepo = null, Func<MediaTypeInfo, Stream> contentCallback = null)
        {
            var mock = new Mock<IMediaService>();
            mediaRepo = mediaRepo ?? Enumerable.Empty<MediaTypeInfo>();
            contentCallback = contentCallback ?? ((MediaTypeInfo m) => null);
            mock
                .Setup(m => m.GetMedia(It.IsAny<Guid>()))
                .Returns((Guid storageId) =>
                {
                    MediaTypeInfo metadata = mediaRepo.FirstOrDefault(m => m.StorageId == storageId);
                    return metadata != null
                    ? new MultimediaContent(contentCallback(metadata), metadata)
                    : null;
                });

            return mock;
        }

        public static Mock<DbSet> GetMockDbSet(ArrayList backingStore = null)
        {
            backingStore = backingStore ?? new ArrayList();

            var result = new Mock<DbSet>();
            result.Setup(r => r.Add(It.IsAny<object>())).Callback((object obj) => backingStore.Add(obj));
            result.Setup(r => r.Attach(It.IsAny<object>())).Callback((object obj) => backingStore.Add(obj));
            result.Setup(r => r.Remove(It.IsAny<object>())).Callback((object obj) => backingStore.Remove(obj));
            result.Setup(r => r.RemoveRange(It.IsAny<IEnumerable>())).Callback((IEnumerable range) =>
            {
                foreach (object item in range ?? Enumerable.Empty<object>())
                {
                    backingStore.Remove(item);
                }
            });

            return result;
        }

        /// <summary>
        /// Returns a mock <see cref="IDbSet{TEntity}"/> using a <see cref="List{T}"/> as a backing store.
        /// Note that the <see cref="IDbSet{TEntity}.Find(object[])"/> method will throw an exception by default and
        /// must be stubbed manually by consuming tests (for the time being).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="backingStore"></param>
        /// <returns></returns>
        public static Mock<DbSet<T>> GetMockDbSet<T>(IList<T> backingStore = null)
            where T : class
        {
            var result = new Mock<DbSet<T>>();
            backingStore = backingStore ?? new List<T>();
            backingStore = backingStore ?? new List<T>();
            IQueryable<T> queryableStore = backingStore.AsQueryable();
            result.As<IQueryable<T>>().Setup(q => q.ElementType).Returns(queryableStore.ElementType);
            result.As<IQueryable<T>>().Setup(q => q.Expression).Returns(queryableStore.Expression);
            result.As<IQueryable<T>>().Setup(q => q.GetEnumerator()).Returns(queryableStore.GetEnumerator);
            result.As<IQueryable<T>>().Setup(q => q.Provider).Returns(queryableStore.Provider);
            result.Setup(r => r.Add(It.IsAny<T>())).Callback((T obj) => backingStore.Add(obj));
            result.Setup(r => r.Attach(It.IsAny<T>())).Callback((T obj) => backingStore.Add(obj));
            result.Setup(r => r.Remove(It.IsAny<T>())).Callback((T obj) => backingStore.Remove(obj));
            result.Setup(r => r.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback((IEnumerable<T> range) =>
            {
                foreach (T item in range ?? Enumerable.Empty<T>())
                {
                    backingStore.Remove(item);
                }
            });
            result.Setup(r => r.Find(It.IsAny<object[]>())).Throws<NotImplementedException>();

            return result;
        }

        public static Mock<DbContext> GetMockDbContext(Func<Type, DbSet> setFactory = null)
        {
            var result = new Mock<DbContext>();

            setFactory = setFactory ?? DefaultSetFactory;
            result.Setup(d => d.Set(It.IsNotNull<Type>())).Returns(setFactory);

            return result;
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
            settingsRepo = settingsRepo ?? new List<Setting>();
            var mockService = new Mock<ISettingsService>();
            mockService.Setup(x => x.ReadSetting(It.IsNotNull<string>())).Returns((string name) =>
            {
                return settingsRepo?.Where(s => s.Name == name).OrderByDescending(s => s.Priority).FirstOrDefault();
            });
            mockService.Setup(x => x.WriteSetting(It.IsAny<Setting>())).Callback((Setting s) =>
            {
                int existingElemIndex = settingsRepo?.IndexOf(s) ?? -1;
                if (existingElemIndex >= 0)
                {
                    settingsRepo.RemoveAt(existingElemIndex);
                }
                settingsRepo?.Add(s);
            });
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

        public static Mock<IRepository<T>> GetMockRepository<T>(List<T> entities = null) where T : class
        {
            entities = entities ?? new List<T>();

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

        public static Mock<IPrincipal> GetMockUser(string name, bool isAuthenticated = true)
        {
            var identity = new Mock<IIdentity>();
            identity.SetupGet(x => x.Name).Returns(name);
            identity.SetupGet(x => x.IsAuthenticated).Returns(isAuthenticated);

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
            result
                .SetupGet(r => r.HttpContext)
                .Returns(GetMockHttpContextBase(request, response).Object);

            result
                .Setup(x => x.RouteData)
                .Returns(new RouteData()
                {
                    Route = new Mock<RouteBase>().Object,
                    RouteHandler = new Mock<IRouteHandler>().Object
                });

            return result;
        }

        public static Mock<HttpContextBase> GetMockHttpContextBase(HttpRequestBase request = null, HttpResponseBase response = null)
        {
            var result = new Mock<HttpContextBase>();
            result.SetupGet(x => x.Request).Returns(request);
            result.SetupGet(x => x.Response).Returns(response);

            return result;
        }

        public static Mock<ControllerBase> GetMockController()
        {
            var controller = new Mock<ControllerBase>();
            controller.SetupAllProperties();

            return controller;
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

        public static Mock<IComposerRepository> GetMockComposerRepository(List<Composer> backingStore = null)
        {
            backingStore = backingStore ?? new List<Composer>();
            var mock = new Mock<IComposerRepository>();
            mock
                .Setup(r => r.Find(It.IsAny<Expression<Func<IComposerNameDto, bool>>>()))
                .Returns<Expression<Func<IComposerNameDto, bool>>>((expr) =>
                {
                    HashSet<Guid> ids = new HashSet<Guid>(backingStore
                        .SelectMany(c =>
                        {
                            IEnumerable<NameRelationalDto> names = c.Name.All().Select(n => new NameRelationalDto() { Composer_Id = c.Id, FullName = n.Value.FullName, Language = n.Key.Name });
                            if (!names.Any())
                            {
                                return new[] { new NameRelationalDto() { Composer_Id = c.Id, FullName = "", Language = "" } };
                            }
                            else
                            {
                                return names;
                            }
                        })
                        .Cast<IComposerNameDto>().AsQueryable()
                        .Where(expr).ToList()
                        .Select(d => (d as NameRelationalDto).Composer_Id));

                    return backingStore.Where(c => ids.Contains(c.Id));
                });

            mock.Setup(r => r.AddOrUpdate(It.IsAny<Composer>())).Callback((Composer c) =>
            {
                if (!backingStore.Any(item => item.Id == c.Id))
                {
                    backingStore.Add(c);
                }
            });

            mock.Setup(r => r.Find(It.IsAny<Guid>())).Returns((Guid id) => backingStore.FirstOrDefault(c => c.Id == id));

            mock.Setup(r => r.Delete(It.IsAny<Guid[]>())).Callback((Guid[] ids) =>
            {
                HashSet<Guid> idsHash = new HashSet<Guid>(ids);
                IEnumerable<int> deleteAtIndexes = backingStore.Select((c, index) => idsHash.Contains(c.Id) ? index : -1).Where(x => x >= 0).Reverse();
                foreach (int index in deleteAtIndexes)
                {
                    backingStore.RemoveAt(index);
                }
            });

            return mock;
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
            mockService.Setup(s => s.Search(It.IsAny<string>(), It.IsAny<CultureInfo>())).Returns<string>(q => composers?.Where(c => c.LocalizedNames.Any(name => name.FullName.Contains(q))).Select(name => new SearchResult() { Header = name.Id.ToString() }));

            return mockService;
        }

        public static WebApplicationSettings GetStandardAppProfile()
        {
            return new WebApplicationSettings()
            {
                LocaleCookieName = "localeCookie",
                LocaleRouteTokenName = "locale",
                LocaleKey = "locale",
                SupportedLanguages = new List<CultureInfo>() { CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("de-DE") }
            };
        }
    }
}
