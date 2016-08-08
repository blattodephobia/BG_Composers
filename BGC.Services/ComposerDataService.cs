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
	internal class ComposerDataService : DbServiceBase, IComposerDataService
	{
		protected IRepository<Composer> Composers { get; private set; }
        protected IRepository<ComposerName> Names { get; private set; }

		public ComposerDataService(IRepository<Composer> composers, IRepository<ComposerName> names)
		{
            Shield.ArgumentNotNull(composers, nameof(composers)).ThrowOnError();
            Shield.ArgumentNotNull(names, nameof(names)).ThrowOnError();
			this.Composers = composers;
            this.Names = names;
		}

		public IList<Composer> GetAllComposers()
		{
			return this.Composers.All().ToList();
		}

        public void Add(Composer composer)
        {
            Shield.ArgumentNotNull(composer, nameof(composer)).ThrowOnError();

            this.Composers.Insert(composer);
            SaveAll();
        }

        public IList<ComposerName> GetNames(CultureInfo culture)
        {
            return Names.All().Where(c => c.LanguageInternal == culture.Name).ToList();
        }
    }
}
