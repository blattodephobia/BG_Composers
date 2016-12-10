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
    public class MultiValueFifoDependencySource<T> : DependencySource<T>
    {
        private Queue<T> queue;

        public MultiValueFifoDependencySource()
        {
            this.queue = new Queue<T>();
        }

        public override bool HasValue => this.queue.Count > 0;

        public override T GetEffectiveValue()
        {
            return HasValue ? queue.First() : default(T);
        }

        public override void SetValue(T value)
        {
            this.queue.Enqueue(value);

            bool raiseEvent = this.queue.Count == 1;
            if (raiseEvent) OnEffectiveValueChanged(default(T));
        }

        public override void UnsetValue()
        {
            if (HasValue)
            {
                T unsetValue = this.queue.Dequeue();
                OnEffectiveValueChanged(unsetValue);
            }
        }
    }
}
