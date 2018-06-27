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

        public ComposersRepository(ComposerTypeMapper typeMapper, ComposerPropertyMapper propertyMapper, DbContext context) : base(typeMapper, propertyMapper, context)
        {
        }

        public IEnumerable<Composer> Find(Expression<Func<IComposerNameDto, bool>> selector)
        {
            Shield.ArgumentNotNull(selector).ThrowOnError();

            Expression<Func<NameRelationalDto, bool>> mappedSelector = _expressionMap.ChangeLambdaType(selector);
            IQueryable<ComposerRelationalDto> queryResult = DbContext.Set<NameRelationalDto>().Where(mappedSelector).Select(name => name.Composer);

            List<Composer> result = new List<Composer>();
            foreach (ComposerRelationalDto dto in queryResult)
            {
                result.Add(TypeMapper.Build(dto));
            }

            return result;
        }
    }
}
