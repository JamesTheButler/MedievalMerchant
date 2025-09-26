#nullable enable
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Common
{
    public static class CollectionExtensions
    {
        public static T? GetRandom<T>(this IList<T>? source)
        {
            if (source == null || source.Count == 0)
            {
                return default;
            }

            var index = Random.Range(0, source.Count);
            return source[index];
        }

        public static T? GetRandom<T>(this IEnumerable<T> source)
        {
            return source.ToList().GetRandom();
        }

        public static string PrettyPrint<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            return "{\n" + string.Join("\n", dict.Select(kvp => $"{kvp.Key}: {kvp.Value}")) + "\n}";
        }

        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : struct
        {
            return source.Where(item => item != null).Cast<T>();
        }

        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : class
        {
            return source.Where(item => item != null).Cast<T>();
        }

        /// <summary>
        /// Returns all possible tuples.
        /// </summary>
        public static IEnumerable<(T first, T second)> GetAllTuples<T>(this IEnumerable<T> source)
        {
            var items = source.ToList();

            for (var i = 0; i < items.Count; i++)
            {
                for (var j = i + 1; j < items.Count; j++)
                {
                    yield return (items[i], items[j]);
                }
            }
        }
    }
}