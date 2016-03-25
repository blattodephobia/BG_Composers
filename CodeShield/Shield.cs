using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShield
{
    public static partial class Shield
    {
        public static Validation<IEnumerable<T>> IsNotNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return new Validation<IEnumerable<T>>(collection, (collection?.Any() ?? false) ? (Func<IEnumerable<T>, Exception>)null : x => new Exception());
        }

        public static Validation<T> ArgumentNotNull<T>(this T obj, string objName = null)
            where T : class
        {
            return new Validation<T>(obj, ReferenceEquals(obj, null) ? (x => new ArgumentNullException(objName)) : (Func<T, Exception>)null);
        }

        public static Validation<T?> ArgumentNotNull<T>(this T? obj, string objName = null)
            where T : struct
        {
            return new Validation<T?>(obj, obj.HasValue ? (Func<T?, Exception>)null : x => new ArgumentNullException(objName));
        }
    }
}
