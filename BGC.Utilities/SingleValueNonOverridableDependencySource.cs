using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    /// <summary>
    /// A <see cref="DependencySource{T}"/> that can hold a single value at a time. Subsequent attempts
    /// to change the value will take no effect, unless that value has been unset.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NonOverridableSingleDependencyValueSource<T> : SingleValueDependencySource<T>
    {
        public override void SetValue(T value)
        {
            if (!HasValue) base.SetValue(value);
        }
    }
}
