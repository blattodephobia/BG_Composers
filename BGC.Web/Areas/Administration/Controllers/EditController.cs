using BGC.Core;
using BGC.Core.Services;
using BGC.Web.Areas.Administration.ViewModels;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace BGC.Web.Areas.Administration.Controllers
{
    public partial class EditController : AdministrationControllerBase
    {
        private static readonly XName ImgTagName = XName.Get("img");

        private IComposerDataService _composersService;
        private ISettingsService settingsService;
        private IArticleContentService _articleStorageService;

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

        public EditController(IComposerDataService composersService, ISettingsService settingsService, IArticleContentService articleStorageService)
        {
            _composersService = composersService.ArgumentNotNull(nameof(composersService)).GetValueOrThrow();
            this.settingsService = settingsService.ArgumentNotNull(nameof(settingsService)).GetValueOrThrow();
            _articleStorageService = articleStorageService.ArgumentNotNull(nameof(articleStorageService)).GetValueOrThrow();
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
                var name = new ComposerName(editedData.Articles[i].FullName, editedData.Articles[i].Language);
                newComposer.LocalizedNames.Add(name);
                newComposer.AddArticle(new ComposerArticle()
                {
                    StorageId = _articleStorageService.StoreEntry(editedData.Articles[i].Content),
                    Composer = newComposer,
                    Language = editedData.Articles[i].Language,
                    LocalizedName = name
                });
            }
            _composersService.AddOrUpdate(newComposer);

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        [Permissions(nameof(IArticleManagementPermission))]
        public virtual ActionResult Update(Guid composerId)
        {
            Composer composer = _composersService.FindComposer(composerId);
            var model = new UpdateComposerViewModel()
            {
                Articles = composer.GetArticles().Select(a => new AddArticleViewModel()
                {
                    Content = _articleStorageService.GetEntry(composer.GetArticle(a.Language).StorageId),
                    FullName = composer.GetName(a.Language).FullName,
                    Language = a.Language
                }).ToList()
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
                ComposerName name = editingComposer.GetName(newArticle.Language);
                if (name.FullName != newArticle.FullName)
                {
                    name.FullName = newArticle.FullName;
                }

                ComposerArticle existingArticle = editingComposer.GetArticle(newArticle.Language);
                if (newArticle.Content != _articleStorageService.GetEntry(existingArticle.StorageId))
                {
                    editingComposer.AddArticle(new ComposerArticle(editingComposer, newArticle.Language)
                    {
                        StorageId = _articleStorageService.StoreEntry(newArticle.Content)
                    });
                }
            }
            _composersService.AddOrUpdate(editingComposer);

            return RedirectToAction(MVC.Administration.Edit.List());
        }
    }
}
