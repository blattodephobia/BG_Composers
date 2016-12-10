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
        private Func<object, IEnumerable<DependencySource<T>>> _dependencySourceAccessor;

        protected IEnumerable<DependencySource<T>> GetDependencySources() => (_dependencySourceAccessor ?? (_dependencySourceAccessor = Expressions.GetPropertyValuesOfTypeAccessor<DependencySource<T>>(GetType()))).Invoke(this);

        protected DefaultValueDependencySource<T> DefaultValue { get; private set; }

        public DependencyValue()
        {
            DefaultValue = DefaultValueDependencySource<T>.DefaultTValueSource;
        }

        public DependencyValue(T defaultValue)
        {
            DefaultValue = new DefaultValueDependencySource<T>(defaultValue);
        }

        public DependencyValue(Func<T> defaultValueFactory)
        {
            Shield.ArgumentNotNull(defaultValueFactory).ThrowOnError();

            DefaultValue = new DefaultValueDependencySource<T>(defaultValueFactory);
        }
    }
}
