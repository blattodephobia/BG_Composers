using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    internal struct TypeDiscoveryCacheKey
    {
        private readonly int _typeHashCode;
        private readonly int _assemblyPredicateHashCode;
        private readonly int _modeHashCode;

        public TypeDiscoveryCacheKey(Type t, Func<Assembly, bool> assemblyPredicate, TypeDiscoveryMode mode) :
            this()
        {
            _typeHashCode = t?.GetHashCode() ?? 0;
            _assemblyPredicateHashCode = assemblyPredicate?.GetHashCode() ?? 0;
            _modeHashCode = mode.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _typeHashCode ^ _assemblyPredicateHashCode ^ _modeHashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj is TypeDiscoveryCacheKey)
            {
                var key = ((TypeDiscoveryCacheKey)obj);
                return key._assemblyPredicateHashCode == _assemblyPredicateHashCode &&
                       key._modeHashCode == _modeHashCode &&
                       key._typeHashCode == _typeHashCode;
            }

            return false;
        }
    }
}
