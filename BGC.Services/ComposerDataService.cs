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

            if (!Composers.All().Any(c => c.Id == composer.Id))
            {
                Composers.Insert(composer);
            }

            var duplicateNames = from name in composer.LocalizedNames
                                 join existingName in Names.All() on name.FullName equals existingName.FullName
                                 where existingName.Composer.Id != composer.Id &&
                                       existingName.LanguageInternal == name.LanguageInternal
                                 group existingName by existingName.LanguageInternal into duplicateName
                                 select duplicateName;
            if (duplicateNames.Any())
            {
                composer.Order = duplicateNames.Max(group => group.Count());
                composer.HasNamesakes = true;
                foreach (Composer namesake in duplicateNames.SelectMany(group => group).Select(name => name.Composer).Distinct())
                {
                    namesake.HasNamesakes = true;
                }
            }

            SaveAll();
        }

        public IList<ComposerName> GetNames(CultureInfo culture) => Names.All().Where(c => c.LanguageInternal == culture.Name).ToList();

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
