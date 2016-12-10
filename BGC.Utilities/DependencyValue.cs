using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public abstract class DependencyValue<T>
    {
        private Func<DependencyValue<T>, IEnumerable<DependencySource<T>>> GenerateDependencySourcesGetter()
        {
            Type currentType = GetType();
            List<PropertyInfo> properties = (from property in currentType.GetProperties()
                                             let priorityAttribute = property.GetCustomAttribute<DependencyPrecedenceAttribute>()
                                             where typeof(DependencySource<T>).IsAssignableFrom(property.PropertyType) && priorityAttribute != null
                                             orderby priorityAttribute.Priority descending
                                             select property).ToList();
            properties.Add(currentType.GetProperty(nameof(DefaultValue), BindingFlags.Instance | BindingFlags.NonPublic));
            _dependencySourceGetter = Expressions.GetPropertyValuesOfTypeAccessor<DependencySource<T>>(currentType, properties);
            return _dependencySourceGetter;
        }

        private Func<DependencyValue<T>, IEnumerable<DependencySource<T>>> _dependencySourceGetter;
        private Func<DependencyValue<T>, IEnumerable<DependencySource<T>>> DependencySourcesGetter => _dependencySourceGetter ?? (_dependencySourceGetter = GenerateDependencySourcesGetter());

        protected IEnumerable<DependencySource<T>> GetDependencySources() => DependencySourcesGetter.Invoke(this);

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
