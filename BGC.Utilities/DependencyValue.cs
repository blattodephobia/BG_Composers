using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public class DependencyValue<T>
    {
        private Func<object, IEnumerable<DependencySource<T>>> dependencySourceAccessor;

        protected IEnumerable<DependencySource<T>> GetDependencySources()
        {
            return this.dependencySourceAccessor.Invoke(this);
        }

        public DependencyValue()
        {
            this.dependencySourceAccessor = Expressions.GetPropertyValuesOfTypeAccessor<DependencySource<T>>(GetType());
        }
    }
}
