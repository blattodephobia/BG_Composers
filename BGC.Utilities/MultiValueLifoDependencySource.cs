using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    /// <summary>
    /// A <see cref="DependencySource{T}"/> that can hold multiple values.
    /// The order in which values take effect and are unset follows the last-in-first-out principle.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiValueLifoDependencySource<T> : DependencySource<T>
    {
        private Stack<T> stack;

        public MultiValueLifoDependencySource()
        {
            this.stack = new Stack<T>();
        }

        public override bool HasValue => this.stack.Count > 0;

        public override T GetEffectiveValue()
        {
            return HasValue ? stack.Peek() : default(T);
        }

        public override void SetValue(T value)
        {
            this.stack.Push(value);

            bool raiseEvent = this.stack.Count == 1;
            if (raiseEvent) OnEffectiveValueChanged(default(T));
        }

        public override void UnsetValue()
        {
            if (HasValue)
            {
                T unsetValue = this.stack.Pop();
                OnEffectiveValueChanged(unsetValue);
            }
        }
    }
}
