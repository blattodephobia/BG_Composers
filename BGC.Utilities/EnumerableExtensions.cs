using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public static class EnumerableExtensions
    {
        public static string ToStringAggregate<T>(this IEnumerable<T> collection, string delimiter = null)
        {
            StringBuilder result = collection.Aggregate(
                new StringBuilder(),
                (sb, current) => sb.AppendFormat("{0}{1}", current, delimiter),
                sb => sb.Length > 0 ? sb.Remove(sb.Length - delimiter?.Length ?? 0, delimiter?.Length ?? 0) : sb);
            return result.ToString();
        }
    }
}
