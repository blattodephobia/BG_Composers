using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public partial class LazyOrderedEnumerable<T> : IOrderedEnumerable<T>
    {
        private static int PivotIndex(int startIndex, int endIndex)
        {
            return startIndex + (endIndex - startIndex) / 2;
        }

        private readonly List<T> _internalCollection;

        public IOrderedEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public LazyOrderedEnumerable(IEnumerable<T> collection)
        {
            _internalCollection = collection.ToList();
        }
    }
}
