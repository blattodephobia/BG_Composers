using CodeShield;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public partial class LazyOrderedEnumerable<T, TKey> : IOrderedEnumerable<T>
    {
        private readonly List<T> _internalCollection;
        private readonly Func<T, TKey> _keySelector;
        private readonly IComparer<TKey> _comparer;
        private readonly bool _descending;

        private int Compare(T x, T y)
        {
            TKey left = _keySelector.Invoke(x);
            TKey right = _keySelector.Invoke(y);
            int comparisonResult = _comparer.Compare(left, right);
            return _descending ? -comparisonResult : comparisonResult;
        }

        IOrderedEnumerable<T> IOrderedEnumerable<T>.CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            return new LazyOrderedEnumerable<T, TKey>(_internalCollection, keySelector, comparer, descending);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Iterator<T>(_internalCollection, this);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public LazyOrderedEnumerable(IEnumerable<T> collection, Func<T, TKey> keySelector, IComparer<TKey> comparer = null, bool descending = false)
        {
            Shield.ArgumentNotNull(keySelector, nameof(keySelector)).ThrowOnError();
            Shield.ArgumentNotNull(collection, nameof(collection)).ThrowOnError();
            Shield.Assert(
                typeof(TKey),
                comparer != null || (typeof(IComparable<TKey>).IsAssignableFrom(typeof(TKey))),
                x => new ArgumentException($"{typeof(TKey)} doesn't implement {typeof(IComparable<TKey>)} and no {typeof(IComparer<TKey>)} has been specified that can handle comparisons.")).ThrowOnError();

            _keySelector = keySelector;
            _comparer = comparer ?? Comparer<TKey>.Default;
            _internalCollection = collection.ToList();
            _descending = descending;
        }
    }
}
