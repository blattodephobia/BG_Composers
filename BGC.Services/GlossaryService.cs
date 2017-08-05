using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BGC.Core.Models;
using CodeShield;
using System.Globalization;

namespace BGC.Services
{
    internal class GlossaryService : DbServiceBase, IGlossaryService
    {
        public IRepository<GlossaryEntry> GlossaryRepo { get; private set; }

        public GlossaryService(IRepository<GlossaryEntry> glossaryRepository)
        {
            Shield.ArgumentNotNull(glossaryRepository, nameof(glossaryRepository)).ThrowOnError();

            GlossaryRepo = glossaryRepository;
        }

        public void AddOrUpdate(GlossaryEntry entry)
        {
            Shield.ArgumentNotNull(entry, nameof(entry)).ThrowOnError();

            GlossaryEntry existingEntry = GlossaryRepo.All().FirstOrDefault(g => g.Id == entry.Id);
            if (existingEntry != null)
            {
                if (!ReferenceEquals(existingEntry, entry))
                {
                    existingEntry.Definitions.Clear();
                    foreach (GlossaryDefinition definition in entry.Definitions)
                    {
                        existingEntry.Definitions.Add(definition);
                    }
                }
            }
            else
            {
                GlossaryRepo.Insert(entry);
            }

            SaveAll();
        }

        public void Delete(GlossaryEntry entry)
        {
            GlossaryRepo.Delete(entry);
            SaveAll();
        }

        public IEnumerable<GlossaryEntry> ListAll() => GlossaryRepo.All().ToList();

        public GlossaryEntry Find(Guid id) => GlossaryRepo.All().FirstOrDefault(g => g.Id == id);
    }
}
