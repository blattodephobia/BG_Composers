using BGC.Core;
using BGC.Core.Services;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace BGC.Services
{
    internal class ComposerDataService : DbServiceBase, IComposerDataService, ISearchService
    {
        protected IRepository<Composer> Composers { get; private set; }
        protected IRepository<ComposerName> Names { get; private set; }

        private IEnumerable<SearchResult> SearchInternal(IEnumerable<string> keywords) =>
            from resultName in (from composer in Composers.All()
                                from name in composer.LocalizedNames
                                where keywords.Any(keyword => name.FullName.Contains(keyword))
                                select name).ToList()
            select new SearchResult(resultName.Composer.Id)
            {
                Header = resultName.FullName,
                ParsedResultXml = null
            };

        public ComposerDataService(IRepository<Composer> composers, IRepository<ComposerName> names)
        {
            Shield.ArgumentNotNull(composers, nameof(composers)).ThrowOnError();
            Shield.ArgumentNotNull(names, nameof(names)).ThrowOnError();

            Composers = composers;
            Names = names;
        }

        public IList<Composer> GetAllComposers() => Composers.All().ToList();

        public void Add(Composer composer)
        {
            Shield.ArgumentNotNull(composer, nameof(composer)).ThrowOnError();

            Composers.Insert(composer);
            SaveAll();
        }

        public IList<ComposerName> GetNames(CultureInfo culture) => Names.All().Where(c => c.LanguageInternal == culture.Name).ToList();

        public IEnumerable<SearchResult> Search(string query) =>
            SearchInternal(
                query
                .ArgumentNotNull().GetValueOrThrow()
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()));

        public Composer FindComposer(long id) => Composers.All().FirstOrDefault(c => c.Id == id);
    }
}
