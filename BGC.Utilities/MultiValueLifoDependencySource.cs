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
    public class MultiValueLifoDependencySource<T> : MultiValueDependencySource<T>
    {
        private Stack<T> _stack;

        public MultiValueLifoDependencySource()
        {
            _stack = new Stack<T>();
        }

        public override bool HasValue => _stack.Count > 0;

        public override T GetEffectiveValue()
        {
            return HasValue ? _stack.Peek() : default(T);
        }

        public override void SetValue(T value)
        {
            _stack.Push(value);

            bool raiseEvent = _stack.Count == 1;
            if (raiseEvent) OnEffectiveValueChanged(default(T));
        }

        public override void UnsetValue()
        {
            if (HasValue)
            {
                T unsetValue = _stack.Pop();
                OnEffectiveValueChanged(unsetValue);
            }
        }
    }
}
