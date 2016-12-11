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

        public override bool HasValue => _hasValue;

        public override T GetEffectiveValue() => _hasValue ? _value : default(T);

        public override void SetValue(T value)
        {
            T oldValue = _value;
            _value = value;
            _hasValue = true;

            OnEffectiveValueChanged(oldValue);
        }

        public override void UnsetValue()
        {
            T oldValue = _value;
            _value = default(T);
            _hasValue = false;

            OnEffectiveValueChanged(oldValue);
        }
    }
}
