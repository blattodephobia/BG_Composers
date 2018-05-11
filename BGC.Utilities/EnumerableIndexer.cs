using CodeShield;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public class EnumerableIndexer<TKey, TValue> : Indexer<TKey, TValue>
    {
        private readonly Func<IEnumerable<KeyValuePair<TKey, TValue>>> _enumerateCallback;

        public EnumerableIndexer(Func<TKey, TValue> getCallback, Action<TKey, TValue> setCallback, Func<IEnumerable<KeyValuePair<TKey, TValue>>> enumerateCallback) :
            base(getCallback, setCallback)
        {
            Shield.ArgumentNotNull(enumerateCallback).ThrowOnError();

            _enumerateCallback = enumerateCallback;
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> All() => _enumerateCallback.Invoke();

        public IEnumerable<TValue> Values() => All().Select(kvp => kvp.Value);
    }
}
