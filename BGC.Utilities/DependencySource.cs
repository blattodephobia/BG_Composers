using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    /// <summary>
    /// Holds one or more values of a given type <typeparamref name="T"/> and determines one of them
    /// as an effective value based on implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DependencySource<T>
    {
        /// <summary>
        /// Gets the value with the highest priority as determined by the implementation of the <see cref="DependencySource{T}"/> or the default one.
        /// </summary>
        /// <returns></returns>
        public abstract T GetEffectiveValue();

        /// <summary>
        /// Sets a <paramref name="value"/> as a potential effective value.
        /// </summary>
        /// <param name="value"></param>
        public abstract void SetValue(T value);

        /// <summary>
        /// Clears a value entry.
        /// </summary>
        public abstract void UnsetValue();

        /// <summary>
        /// Returns <value>true</value> when the current <see cref="DependencySource{T}"/> holds values set by its <see cref="SetValue(T)"/> method.
        /// </summary>
        public abstract bool HasValue { get; }

        /// <summary>
        /// Specifies whether the default value (null for reference types) will be considered as a valid value for this source.
        /// </summary>
        public bool IsDefaultValueValid { get; protected set; }

        /// <summary>
        /// Raised when a set/unset operation has resulted in the effective value of
        /// the current <see cref="DependencySource{T}"/> being changed.
        /// </summary>
        public event Action<object, EffectiveValueChangedEventArgs<T>> EffectiveValueChanged;

        protected virtual void OnEffectiveValueChanged(T oldValue)
        {
            OnEffectiveValueChanged(oldValue, GetEffectiveValue());
        }

        protected virtual void OnEffectiveValueChanged(T oldValue, T newValue)
        {
            EffectiveValueChanged?.Invoke(this, new EffectiveValueChangedEventArgs<T>(oldValue, newValue));
        }

        protected DependencySource(bool isDefaultValueValid = true)
        {
            IsDefaultValueValid = isDefaultValueValid;
        }
    }
}
