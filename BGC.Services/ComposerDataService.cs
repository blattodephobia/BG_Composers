﻿using BGC.Core;
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
        private readonly IComposerRepository _composersRepo;

        private IEnumerable<SearchResult> SearchInternal(IEnumerable<string> keywords, CultureInfo locale)
        {
            IEnumerable<Composer> results =
                from result in _composersRepo.Find(name => keywords.Any(keyword => name.FullName.ToUpper().Contains(keyword.ToUpper())))
                where result.FindArticle(locale) != null
                select result;

            return results.GroupBy(c => c.FindArticle(locale), ComposerArticle.Comparers.ByComposerEqualityComparer).Select(group => new ComposerSearchResult(group.First(), locale));
        }

        public ComposerDataService(IComposerRepository repo)
        {
            Shield.ArgumentNotNull(repo, nameof(repo)).ThrowOnError();

            _composersRepo = repo;
        }

        public IEnumerable<Composer> GetAllComposers() => _composersRepo.Find(selector: dto => true);

        public void AddOrUpdate(Composer composer)
        {
            Shield.ArgumentNotNull(composer, nameof(composer)).ThrowOnError();

            HashSet<string> currentNames = new HashSet<string>(composer.Name.All().Select(n => n.Value.FullName));
            List<Composer> duplicateComposers = new List<Composer>() { composer };
            duplicateComposers.AddRange(_composersRepo.Find(name => currentNames.Contains(name.FullName)));
            duplicateComposers.Sort((c1, c2) => DateTime.Compare(c1.DateAdded ?? DateTime.MaxValue, c2.DateAdded ?? DateTime.MaxValue)); // sorting by DateTime.MaxValue to make sure a composer who hasn't been added yet is last in the list

            if (duplicateComposers.Any())
            {
                for (int i = 0; i < duplicateComposers.Count; i++)
                {
                    duplicateComposers[i].Order = i;
                    duplicateComposers[i].HasNamesakes = duplicateComposers.Count > 1;
                }
            }

            _composersRepo.AddOrUpdate(composer);
            _composersRepo.SaveChanges();
        }

        public IList<ComposerName> GetNames(CultureInfo culture)
        {
            Shield.ArgumentNotNull(culture).ThrowOnError();

            return _composersRepo.Find(name => name.Language == culture.Name).Select(c => c.Name[culture]).ToList();
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

        public Composer FindComposer(Guid id) => _composersRepo.Find(id);
    }
}
