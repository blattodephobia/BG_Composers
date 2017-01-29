using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    /// <summary>
    /// A <see cref="DependencySource{T}"/> that can hold a single value at a time.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleValueDependencySource<T> : DependencySource<T>
    {
        private T _value;
        private bool _hasValue;

        protected T ValueInternal
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        public override bool HasValue => _hasValue;

        public override T GetEffectiveValue() => HasValue ? ValueInternal : default(T);

        public override void SetValue(T value)
        {
            T oldValue = ValueInternal;
            ValueInternal = value;
            _hasValue = IsDefaultValueValid || (!ValueInternal?.Equals(default(T)) ?? false);

            OnEffectiveValueChanged(oldValue);
        }

        public override void UnsetValue()
        {
            T oldValue = ValueInternal;
            ValueInternal = default(T);
            _hasValue = false;

            OnEffectiveValueChanged(oldValue);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SingleValueDependencySource{T}"/> class.
        /// </summary>
        /// <param name="isDefaultValueAllowed">Specifies whether the default value (null for reference types) will be considered as a valid value for this source.</param>
        public SingleValueDependencySource(bool isDefaultValueAllowed = true) :
            base(isDefaultValueAllowed)
        {

        }
    }
}
