using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BGC.Core;
using BGC.Data.Relational.Mappings;
using BGC.Utilities;
using CodeShield;

namespace BGC.Data.Relational.Repositories
{
    internal class ComposersRepository : EntityFrameworkRepository<Guid, Composer, ComposerRelationalDto>, IComposerRepository
    {
        private readonly LambdaTypeSubstitution<IComposerNameDto, NameRelationalDto> _expressionMap = new LambdaTypeSubstitution<IComposerNameDto, NameRelationalDto>();

        public ComposersRepository(ComposerTypeMapper typeMapper, DbContext context) : base(typeMapper, context)
        {
        }

        protected override void AddOrUpdateInternal(Composer entity)
        {
            bool dateAddedChanged = false;
            try
            {
                if (entity.DateAdded == null)
                {
                    entity.DateAdded = DateTime.UtcNow;
                    dateAddedChanged = true;
                }

                base.AddOrUpdateInternal(entity);
            }
            catch
            {
                if (dateAddedChanged)
                {
                    entity.DateAdded = null;
                }

                throw;
            }
        }

        public IEnumerable<Composer> Find(Expression<Func<IComposerNameDto, bool>> selector)
        {
            Shield.ArgumentNotNull(selector).ThrowOnError();

            Expression<Func<NameRelationalDto, bool>> mappedSelector = _expressionMap.ChangeLambdaType(selector);
            IQueryable<ComposerRelationalDto> query = DbContext.Set<NameRelationalDto>().Where(mappedSelector).GroupBy(n => n.Composer_Id).Select(group => group.FirstOrDefault().Composer);
            IEnumerable<Composer> result = ExecuteAndMapQuery(query);

            return result;
        }
    }
}
