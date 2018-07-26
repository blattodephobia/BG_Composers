using BGC.Core;
using BGC.Core.Services;
using BGC.Web.Areas.Administration.ViewModels;
using BGC.Web.Areas.Public.Controllers;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace BGC.Web.Areas.Administration.Controllers
{
    public partial class EditController : AdministrationControllerBase
    {
        private static readonly XName ImgTagName = XName.Get("img");

        private IComposerDataService _composersService;
        private ISettingsService _settingsService;
        private IArticleContentService _articleStorageService;
        private IMediaService _mediaService;

        private static IEnumerable<XElement> GetImageTags(XElement htmlRoot)
        {
            List<XElement> result = new List<XElement>();
            Queue<XElement> searchQueue = new Queue<XElement>();
            searchQueue.Enqueue(htmlRoot);
            while (searchQueue.Count > 0)
            {
                XElement current = searchQueue.Dequeue();
                if (current.Name == ImgTagName)
                {
                    result.Add(current);
                }

                foreach (XElement child in current.Elements())
                {
                    searchQueue.Enqueue(child);
                }
            }

            return result;
        }

        private static string ExtractQueryString(Uri uri)
        {
            if (uri.IsAbsoluteUri) return uri.Query;

            string fullUri = uri.ToString();
            return fullUri.Substring(fullUri.IndexOf('?') + 1);
        }

        private string GetImageUrl(MediaTypeInfo media)
        {
            return media.ExternalLocation ?? ResourcesController.GetLocalResourceUri(Url, media.StorageId).PathAndQuery;
        }

        private List<MediaTypeInfo> IdentifyImages(AddComposerViewModel vm) => vm.Images.Select(s => ToMediaTypeInfo(s)).ToList();

        private MediaTypeInfo ToMediaTypeInfo(ImageViewModel s)
        {
            var uri = new Uri(s.Location, UriKind.RelativeOrAbsolute);
            MediaTypeInfo result = null;
            if (!uri.IsAbsoluteUri || uri.Host == Request.Url.Host)
            {
                NameValueCollection query = HttpUtility.ParseQueryString(ExtractQueryString(uri));
                Guid id = Guid.Parse(query[MVC.Public.Resources.GetParams.resourceId]);
                result = _mediaService.GetMedia(id)?.Metadata;
            }
            else
            {
                result = MediaTypeInfo.NewExternalMedia(uri.AbsoluteUri, new ContentType("image/*"));
            }
            return result;
        }

        public EditController(IComposerDataService composersService, ISettingsService settingsService, IArticleContentService articleStorageService, IMediaService mediaService)
        {
            _composersService = composersService.ArgumentNotNull(nameof(composersService)).GetValueOrThrow();
            _settingsService = settingsService.ArgumentNotNull(nameof(settingsService)).GetValueOrThrow();
            _articleStorageService = articleStorageService.ArgumentNotNull(nameof(articleStorageService)).GetValueOrThrow();
            _mediaService = mediaService.ArgumentNotNull(nameof(mediaService)).GetValueOrThrow();
        }

        [Permissions(nameof(IArticleManagementPermission))]
        public virtual ActionResult List()
        {
            var vm = new ListArticlesViewModel(_composersService.GetAllComposers());

            return View(vm);
        }

        [HttpGet]
        [Permissions(nameof(IArticleManagementPermission))]
        public virtual ActionResult Add()
        {
            IEnumerable<AddArticleViewModel> articlesRequired = from language in ApplicationProfile.SupportedLanguages
                                                                orderby language.Name
                                                                select new AddArticleViewModel() { Language = language };

            var vm = new AddComposerViewModel(articlesRequired);
            return View(vm);
        }

        [HttpPost]
        [ActionName(nameof(Add))]
        [Permissions(nameof(IArticleManagementPermission))]
        public virtual ActionResult Add_Post(AddComposerViewModel editedData)
        {
            Composer newComposer = new Composer();
            for (int i = 0; i < editedData.Articles.Count; i++)
            {
                var name = new ComposerName(editedData.Articles[i].FullName, editedData.Articles[i].Language) { Composer = newComposer };
                newComposer.LocalizedNames.Add(name);
                newComposer.AddArticle(new ComposerArticle()
                {
                    StorageId = _articleStorageService.StoreEntry(editedData.Articles[i].Content),
                    Composer = newComposer,
                    Language = editedData.Articles[i].Language,
                    LocalizedName = name
                });
            }

            newComposer.Profile = newComposer.Profile ?? new ComposerProfile();
            newComposer.Profile.Media = IdentifyImages(editedData);

            _composersService.AddOrUpdate(newComposer);

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        [Permissions(nameof(IArticleManagementPermission))]
        public virtual ActionResult Update(Guid composerId)
        {
            Composer composer = _composersService.FindComposer(composerId);
            IEnumerable<AddArticleViewModel> articles = composer.GetArticles().OrderBy(a => a.Language.Name).Select(a => new AddArticleViewModel()
            {
                Content = _articleStorageService.GetEntry(composer.GetArticle(a.Language).StorageId),
                FullName = composer.Name[a.Language].FullName,
                Language = a.Language,
            });
            IEnumerable<ImageViewModel> imageSources = composer.Profile?.Images().Select(m => new ImageViewModel(GetImageUrl(m))) ?? Enumerable.Empty<ImageViewModel>();
            var model = new UpdateComposerViewModel(articles, imageSources)
            {
                Order = composer.HasNamesakes ? (int?)composer.Order : null,
            };
            return View(model);
        }

        [HttpPost]
        [ActionName(nameof(Update))]
        [Permissions(nameof(IArticleManagementPermission))]
        public virtual ActionResult Update_Post(UpdateComposerViewModel editedData)
        {
            Composer editingComposer = _composersService.FindComposer(editedData.ComposerId);

            foreach (AddArticleViewModel newArticle in editedData.Articles)
            {
                ComposerName name = editingComposer.Name[newArticle.Language];
                if (name.FullName != newArticle.FullName)
                {
                    name.FullName = newArticle.FullName;
                }

                ComposerArticle existingArticle = editingComposer.GetArticle(newArticle.Language);
                if (newArticle.Content != _articleStorageService.GetEntry(existingArticle.StorageId))
                {
                    editingComposer.AddArticle(new ComposerArticle(editingComposer, name, newArticle.Language)
                    {
                        StorageId = _articleStorageService.StoreEntry(newArticle.Content)
                    });
                }
            }

            ComposerProfile profile = editingComposer.Profile ?? new ComposerProfile();
            profile.Media = IdentifyImages(editedData);

            editingComposer.Profile = profile;
            _composersService.AddOrUpdate(editingComposer);

            return RedirectToAction(MVC.Administration.Edit.List());
        }
    }
}
