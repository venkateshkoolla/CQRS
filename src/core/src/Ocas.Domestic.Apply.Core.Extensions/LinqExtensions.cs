using System;
using System.Collections.Generic;

namespace Ocas.Domestic.Apply.Core.Extensions
{
    public static class LinqExtensions
    {
        public static bool AllEqual<T>(this IEnumerable<T> source, IEqualityComparer<T> equalityComparer = null)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    return true;
                }

                var value = enumerator.Current;
                var comparer = equalityComparer ?? EqualityComparer<T>.Default;

                while (enumerator.MoveNext())
                {
                    if (!comparer.Equals(value, enumerator.Current))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public static bool AllUnique<T>(this IEnumerable<T> source, IEqualityComparer<T> equalityComparer = null)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    return true;
                }

                var value = enumerator.Current;
                var comparer = equalityComparer ?? EqualityComparer<T>.Default;

                while (enumerator.MoveNext())
                {
                    if (comparer.Equals(value, enumerator.Current))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            var retVal = 0;

            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }

            return -1;
        }
    }
}
