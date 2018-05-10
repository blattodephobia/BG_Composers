using CodeShield;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public class Indexer<TKey, TValue>
    {
        private readonly Func<TKey, TValue> _getCallback;
        private readonly Action<TKey, TValue> _setCallback;

        public TValue this[TKey key]
        {
            get
            {
                return _getCallback.Invoke(key);
            }

            set
            {
                _setCallback.Invoke(key, value);
            }
        }

        public Indexer(Func<TKey, TValue> getCallback, Action<TKey, TValue> setCallback)
        {
            _getCallback = Shield.ArgumentNotNull(getCallback).GetValueOrThrow();
            _setCallback = Shield.ArgumentNotNull(setCallback).GetValueOrThrow();
        }
    }
}
