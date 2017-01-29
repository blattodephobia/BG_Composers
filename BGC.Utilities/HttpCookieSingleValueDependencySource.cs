using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BGC.Utilities
{
    /// <summary>
    /// A <see cref="SingleValueDependencySource{T}"/> that uses a <see cref="HttpCookie"/> as a backing store.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpCookieSingleValueDependencySource<T> : SingleValueDependencySource<T>
    {
        private readonly HttpCookie _cookieStore;
        private readonly string _keyName;
        private readonly TypeConverter _typeConverter;

        public override void SetValue(T value)
        {
            base.SetValue(value);
            _cookieStore[_keyName] = _typeConverter.ConvertToString(value);
        }

        public override bool HasValue => _cookieStore[_keyName] != null;

        public override T GetEffectiveValue()
        {
            if (!base.HasValue)
            {
                string cookieValue = _cookieStore[_keyName];
                ValueInternal = cookieValue != null
                    ? (T)_typeConverter.ConvertFromString(_cookieStore[_keyName])
                    : default(T);
            }

            return base.GetEffectiveValue();
        }

        public override void UnsetValue()
        {
            _cookieStore[_keyName] = null;
            base.UnsetValue();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="HttpCookieSingleValueDependencySource{T}"/>.
        /// </summary>
        /// <param name="cookie">The cookie to use as a backing store.</param>
        /// <param name="keyName">The name of the key under which values will be stored.</param>
        public HttpCookieSingleValueDependencySource(HttpCookie cookie, string keyName, TypeConverter typeToStringConverter)
        {
            Shield.ArgumentNotNull(cookie).ThrowOnError();
            Shield.ArgumentNotNull(typeToStringConverter).ThrowOnError();
            Shield.Assert(typeToStringConverter, typeToStringConverter.CanConvertFrom(typeof(string)), (t) => new InvalidOperationException($"The provided {typeof(TypeConverter)} cannot convert from {typeof(string)}")).ThrowOnError();
            Shield.Assert(typeToStringConverter, typeToStringConverter.CanConvertTo(typeof(string)), t => new InvalidOperationException($"The provided {typeof(TypeConverter)} cannot convert to {typeof(string)}")).ThrowOnError();
            Shield.ArgumentNotNull(keyName).ThrowOnError();

            _cookieStore = cookie;
            _keyName = keyName;
            _typeConverter = typeToStringConverter;
        }

        public override string ToString() => _cookieStore[_keyName];
    }
}
