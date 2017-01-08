using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    /// <summary>
    /// A <see cref="DependencySource{T}"/> that can hold multiple values.
    /// The order in which values take effect and are unset follows the first-in-first-out principle.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiValueFifoDependencySource<T> : MultiValueDependencySource<T>
    {
        private Queue<T> _queue;

        public MultiValueFifoDependencySource()
        {
            _queue = new Queue<T>();
        }

        public override bool HasValue => _queue.Count > 0;

        public override T GetEffectiveValue() => HasValue ? _queue.First() : default(T);

        public override void SetValue(T value)
        {
            _queue.Enqueue(value);

            bool raiseEvent = _queue.Count == 1;
            if (raiseEvent) OnEffectiveValueChanged(default(T));
        }

        public override void UnsetValue()
        {
            if (HasValue)
            {
                T unsetValue = _queue.Dequeue();
                OnEffectiveValueChanged(unsetValue);
            }
        }
    }
}
