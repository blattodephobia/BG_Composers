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
        private T value;
        private bool hasValue;

        public override bool HasValue => this.hasValue;

        public override T GetEffectiveValue()
        {
            return this.hasValue ? this.value : default(T);
        }

        public override void SetValue(T value)
        {
            this.value = value;
            this.hasValue = true;
        }

        public override void UnsetValue()
        {
            T oldValue = this.value;
            this.value = default(T);
            this.hasValue = false;

            OnEffectiveValueChanged(oldValue);
        }
    }
}
