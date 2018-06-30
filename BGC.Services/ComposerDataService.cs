using BGC.Core;
using BGC.Core.Services;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using BGC.Data;

namespace BGC.Services
{
    internal class ComposerDataService : DbServiceBase, IComposerDataService, ISearchService
    {
        protected IComposerRepository ComposersRepo { get; private set; }
        protected IRepository<Composer> Composers { get; private set; }
        protected IRepository<ComposerName> Names { get; private set; }

        private IEnumerable<SearchResult> SearchInternal(IEnumerable<string> keywords, CultureInfo locale)
        {
            IEnumerable<Composer> results = ComposersRepo.Find(name => keywords.Any(keyword => name.FullName.ToLowerInvariant().Contains(keyword.ToLowerInvariant())));
            return results.Select(composer => new SearchResult(composer.Id)
            {
                Header = composer.Name[locale].FullName,
                ParsedResultXml = null
            });
        }

        public ComposerDataService(IRepository<Composer> composers, IRepository<ComposerName> names, IComposerRepository repo)
        {
            Shield.ArgumentNotNull(composers, nameof(composers)).ThrowOnError();
            Shield.ArgumentNotNull(names, nameof(names)).ThrowOnError();
            Shield.ArgumentNotNull(repo, nameof(repo)).ThrowOnError();

            Composers = composers;
            Names = names;
            ComposersRepo = repo;
        }

        public IEnumerable<Composer> GetAllComposers() => ComposersRepo.Find(selector: dto => true);

        public void AddOrUpdate(Composer composer)
        {
            Shield.ArgumentNotNull(composer, nameof(composer)).ThrowOnError();

            HashSet<string> currentNames = new HashSet<string>(composer.Name.All().Select(n => n.Value.FullName));
            List<Composer> duplicateComposers = new List<Composer>() { composer };
            duplicateComposers.AddRange(ComposersRepo.Find(name => currentNames.Contains(name.FullName)));
            duplicateComposers.Sort((c1, c2) => DateTime.Compare(c1.DateAdded ?? DateTime.MaxValue, c2.DateAdded ?? DateTime.MaxValue)); // sorting by DateTime.MaxValue to make sure a composer who hasn't been added yet is last in the list

            if (duplicateComposers.Any())
            {
                for (int i = 0; i < duplicateComposers.Count; i++)
                {
                    duplicateComposers[i].Order = i;
                    duplicateComposers[i].HasNamesakes = duplicateComposers.Count > 1;
                }
            }

            ComposersRepo.AddOrUpdate(composer);
            SaveAll();
        }

        public IList<ComposerName> GetNames(CultureInfo culture)
        {
            Shield.ArgumentNotNull(culture).ThrowOnError();

            return ComposersRepo.Find(name => name.Language == culture.Name).Select(c => c.Name[culture]).ToList();
        }

        public IEnumerable<SearchResult> Search(string query, CultureInfo locale)
        {
            Shield.ArgumentNotNull(query).ThrowOnError();
            Shield.ArgumentNotNull(locale).ThrowOnError();

            IEnumerable<SearchResult> searchResult = SearchInternal(
                keywords: query.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()),
                locale: locale);
            return searchResult;
        }

        public Composer FindComposer(Guid id) => Composers.All().FirstOrDefault(c => c.Id == id);
    }
}
