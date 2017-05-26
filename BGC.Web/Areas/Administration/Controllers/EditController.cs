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
        private IArticleContentService articleStorageService;

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
            this._composersService = composersService.ArgumentNotNull(nameof(composersService)).GetValueOrThrow();
            this.settingsService = settingsService.ArgumentNotNull(nameof(settingsService)).GetValueOrThrow();
            this.articleStorageService = articleStorageService.ArgumentNotNull(nameof(articleStorageService)).GetValueOrThrow();
        }

        public virtual ActionResult List()
        {
            var vm = new ListArticlesViewModel(_composersService.GetAllComposers());

            return View(vm);
        }

        [HttpGet]
        public virtual ActionResult Add()
        {
            IEnumerable<AddArticleViewModel> articlesRequired = from language in ApplicationProfile.SupportedLanguages
                                                                select new AddArticleViewModel() { Language = language };

            var vm = new AddComposerViewModel(articlesRequired);
            return View(vm);
        }

        [HttpPost]
        [ActionName(nameof(Add))]
        public virtual ActionResult Add_Post(IList<AddArticleViewModel> editedData)
        {
            Composer newComposer = new Composer();
            for (int i = 0; i < editedData.Count; i++)
            {
                var name = new ComposerName(editedData[i].FullName, editedData[i].Language);
                newComposer.LocalizedNames.Add(name);
                newComposer.Articles.Add(new ComposerArticle()
                {
                    StorageId = this.articleStorageService.StoreEntry(editedData[i].Content),
                    Composer = newComposer,
                    Language = editedData[i].Language,
                    LocalizedName = name
                });
            }
            _composersService.Add(newComposer);

            return RedirectToAction(nameof(List));
        }
    }
}
