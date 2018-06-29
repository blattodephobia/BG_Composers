using BGC.Data;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    internal class SingletonRepository<T> : IRepository<T>
        where T : class, new()
    {
        private IRepository<T> baseRepository;

        public IUnitOfWork UnitOfWork => this.baseRepository.UnitOfWork;

        public IQueryable<T> All() => this.baseRepository.All();

        public void Delete(T entity) => this.baseRepository.Delete(entity);

        public void Dispose() => this.baseRepository.Dispose();

        public TDerived GetInstance<TDerived>()
            where TDerived : T, new()
        {
            TDerived result = this.baseRepository.All().OfType<TDerived>().FirstOrDefault();
            if (result == null)
            {
                Insert(result = new TDerived());
            }

            UnitOfWork?.SaveChanges();
            return result;
        }

        public void Insert(T entity)
        {
            this.baseRepository.Insert(entity);
        }

        public SingletonRepository(IRepository<T> baseRepository)
        {
            this.baseRepository = Shield.ArgumentNotNull(baseRepository, nameof(baseRepository)).GetValueOrThrow();
        }
    }
}
