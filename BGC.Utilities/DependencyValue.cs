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
        /// <summary>
        /// Gets all properties of type <see cref="DependencySource{T}"/> or inheriting from it except the <see cref="DefaultValue"/> property. 
        /// </summary>
        /// <returns></returns>
        private Func<DependencyValue<T>, IEnumerable<DependencySource<T>>> GenerateDependencySourcesGetter()
        {
            Type currentType = GetType();
            List<PropertyInfo> properties = (from property in currentType.GetProperties()
                                             let priorityAttribute = property.GetCustomAttribute<DependencyPrecedenceAttribute>()
                                             where typeof(DependencySource<T>).IsAssignableFrom(property.PropertyType) && priorityAttribute != null
                                             orderby priorityAttribute.Priority descending
                                             select property).ToList();
            _dependencySourceGetter = Expressions.GetPropertyValuesOfTypeAccessor<DependencySource<T>>(currentType, properties);
            return _dependencySourceGetter;
        }

        private Func<DependencyValue<T>, IEnumerable<DependencySource<T>>> _dependencySourceGetter;
        private Func<DependencyValue<T>, IEnumerable<DependencySource<T>>> DependencySourcesGetter => _dependencySourceGetter ?? (_dependencySourceGetter = GenerateDependencySourcesGetter());

        private List<DependencySource<T>> _sources;
        private bool _hasEffectiveValue;

        private List<DependencySource<T>> InitializeSources(List<DependencySource<T>> sources)
        {
            for (int i = 0; i < sources.Count; i++)
            {
                if (sources[i] != null)
                {
                    sources[i].EffectiveValueChanged += OnDependencySourceValueChanged;
                }
            }

            _sources = sources;

            return sources;
        }

        private void SetEffectiveValue(IEnumerable<DependencySource<T>> sources)
        {
            DependencySource<T> validSource = (from source in sources ?? Enumerable.Empty<DependencySource<T>>()
                                               where source == DefaultValue || (source?.HasValue ?? false) && (CoerceValue(source.GetEffectiveValue())?.Equals(source.GetEffectiveValue()) ?? false) // ignore sources that are null, empty or contain invalid values
                                               select source).First();
            _effectiveValue = validSource.GetEffectiveValue();
            _hasEffectiveValue = true;
        }

        protected virtual T CoerceValue(T value)
        {
            return value;
        }

        protected List<DependencySource<T>> GetDependencySources()
        {
            List<DependencySource<T>> result = new List<DependencySource<T>>(DependencySourcesGetter.Invoke(this));
            result.Add(DefaultValue);
            return result;
        }

        protected DefaultValueDependencySource<T> DefaultValue { get; private set; }

        protected void OnDependencySourceValueChanged(object sender, EffectiveValueChangedEventArgs<T> args)
        {
            SetEffectiveValue(_sources ?? InitializeSources(GetDependencySources()));
        }

        private T _effectiveValue;
        public T EffectiveValue
        {
            get
            {
                if (_hasEffectiveValue)
                {
                    return _effectiveValue;
                }
                else
                {
                    SetEffectiveValue(InitializeSources(GetDependencySources()));
                    return _effectiveValue;
                }
            }
        }

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
