using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public abstract class MultiValueDependencySource<T> : DependencySource<T>
    {
        public void SetValueRange(IEnumerable<T> values)
        {
            Shield.ArgumentNotNull(values).ThrowOnError();

            foreach (T value in values)
            {
                SetValue(value);
            }
        }
    }
}
