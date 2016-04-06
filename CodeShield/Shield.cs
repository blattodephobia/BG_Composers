using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShield
{
    public static partial class Shield
    {
        /// <summary>
        /// Returns a <see cref="Validation{T}"/> object that will throw an <see cref="InvalidOperationException"/> if
        /// the <param name="collection">collection is null or empty.</param>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objName">The name of the variable to validate.</param>
        /// <returns></returns>
        public static Validation<IEnumerable<T>> IsNotNullOrEmpty<T>(this IEnumerable<T> collection, string objName = null)
        {
            return new Validation<IEnumerable<T>>(collection, (collection?.Any() ?? false) ? (Func<IEnumerable<T>, Exception>)null : x => new InvalidOperationException("Collection cannot be null or empty." + (objName != null ? $"Object name: {objName}" : "")));
        }

        /// <summary>
        /// Returns a <see cref="Validation{T}"/> object that will throw an <see cref="ArgumentNullException"/> if
        /// <paramref name="obj"/> is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="objName">The name of the argument to validate.</param>
        /// <returns></returns>
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

        public static Validation<T> ValueNotNull<T>(this T obj, string objName = null)
            where T : class
        {
            return new Validation<T>(obj, ReferenceEquals(obj, null) ? (x => new InvalidOperationException($"Value cannot be null: {objName}")) : (Func<T, Exception>)null);
        }

        public static Validation<T?> ValueNotNull<T>(this T? obj, string objName = null)
            where T : struct
        {
            return new Validation<T?>(obj, obj.HasValue ? (Func<T?, Exception>)null : x => new InvalidOperationException($"Value cannot be null: {objName}"));
        }

        public static Validation<T> Assert<T>(this T value, bool condition, string message)
        {
            return new Validation<T>(value, condition ? (Func<T, Exception>)null : x => new Exception(message));
        }

        public static Validation<T> Assert<T>(this T value, bool condition, Func<T, Exception> exceptionProvider)
        {
            return new Validation<T>(value, condition ? (Func<T, Exception>)null : exceptionProvider ?? (x => new Exception("Assert failed")));
        }
    }
}
