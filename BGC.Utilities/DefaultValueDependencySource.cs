using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    /// <summary>
    /// A <see cref="DependencySource{T}"/> that provides a single, immutable value during its lifetime. The value may be instantiated in a lazy manner.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultValueDependencySource<T> : DependencySource<T>
    {
        public static readonly DefaultValueDependencySource<T> DefaultTValueSource = new DefaultValueDependencySource<T>();

        private T value;
        private Func<T> valueProvider;
        private bool isInitialized;

        public override bool HasValue => true;

        public override T GetEffectiveValue()
        {
            if (!this.isInitialized)
            {
                this.value = this.valueProvider.Invoke();
                this.isInitialized = true;
            }

            return this.value;
        }

        public override void SetValue(T value)
        {
            throw new NotSupportedException($"{nameof(DefaultValueDependencySource<T>)} can only specify a value once, when its initialized with its constructor.");
        }

        public override void UnsetValue()
        {
            // this method is blank on purpose, because implementations of DependencySource<T>
            // are expected to fall back to a default value, when UnsetValue is called and no
            // other values are stored
        }

        public DefaultValueDependencySource() :
            this(default(T))
        {
        }

        public DefaultValueDependencySource(T value)
        {
            this.value = value;
            this.isInitialized = true;
        }

        public DefaultValueDependencySource(Func<T> valueProvider)
        {
            this.valueProvider = Shield.ArgumentNotNull(valueProvider, nameof(valueProvider)).GetValueOrThrow();
        }
    }
}
