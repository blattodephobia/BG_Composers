using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Calls the <see cref="object.ToString"/> method of all elements in <paramref name="collection"/> and stores the result in a single string by using the specified delimiter. Null values are omitted.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="delimiter">The delimiter that will separate the value of each item's string representation.</param>
        /// <returns></returns>
        public static string ToStringAggregate<T>(this IEnumerable<T> collection, string delimiter = null) => ToStringAggregate(collection, x => x?.ToString(), delimiter);

        /// <summary>
        /// Aggregates the string representation of all items in a collection in a single string by using the specified delimiter and string selector.
        /// Null values are omitted.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="delimiter">The delimiter that will separate the value of each item's string representation.</param>
        /// <returns></returns>
        public static string ToStringAggregate<T>(this IEnumerable<T> collection, Func<T, string> toStringSelector, string delimiter = null)
        {
            Shield.ArgumentNotNull(collection, nameof(collection)).ThrowOnError();
            Shield.ArgumentNotNull(toStringSelector, nameof(toStringSelector)).ThrowOnError();

            StringBuilder result = collection.Select(toStringSelector).Where(s => !string.IsNullOrEmpty(s)).Aggregate(
                new StringBuilder(),
                (sb, current) => sb.AppendFormat("{0}{1}", current, delimiter),
                sb => sb.Length > 0 ? sb.Remove(sb.Length - delimiter?.Length ?? 0, delimiter?.Length ?? 0) : sb);
            return result.ToString();
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            Shield.ArgumentNotNull(collection).ThrowOnError();
            Shield.ArgumentNotNull(items).ThrowOnError();

            List<T> list = collection as List<T>;
            if (list != null)
            {
                list.AddRange(items);
            }
            else
            {
                foreach (T item in items)
                {
                    collection.Add(item);
                }
            }
        }

        /// <summary>
        /// Adds all items from <paramref name="items"/> that are not present in the <paramref name="collection"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        public static void AddMissingRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            Shield.ArgumentNotNull(collection).ThrowOnError();
            Shield.ArgumentNotNull(items).ThrowOnError();
            
            foreach (T item in items)
            {
                if (!collection.Contains(item))
                {
                    collection.Add(item);
                }
            }
        }
    }
}
