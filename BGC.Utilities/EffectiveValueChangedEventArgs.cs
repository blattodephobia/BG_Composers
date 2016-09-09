using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public class EffectiveValueChangedEventArgs<T> : EventArgs
    {
        public T OldValue { get; private set; }

        public T NewValue { get; private set; }

        public EffectiveValueChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
