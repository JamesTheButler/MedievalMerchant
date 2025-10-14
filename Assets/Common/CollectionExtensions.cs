#nullable enable
using System;
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

        public static int IndexOf<T>(this List<T> source, Func<T, bool> predicate)
        {
            var firstFit = source.FirstOrDefault(predicate);
            if (firstFit == null) return -1;
            return source.IndexOf(firstFit);
        }

        public static T[] AsArray<T>(this T value)
        {
            return new[] { value };
        }

        public static List<T> AsList<T>(this T value)
        {
            return new List<T> { value };
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            return source.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public static void AddValues<TKey, TValue>(
            this Dictionary<TKey, TValue> dict,
            IEnumerable<TValue> source,
            Func<TValue, TKey> keySelector)
        {
            foreach (var item in source)
            {
                dict.Add(keySelector(item), item);
            }
        }

        public static void AddKeys<TKey, TValue>(
            this Dictionary<TKey, TValue> dict,
            IEnumerable<TKey> source,
            Func<TKey, TValue> keySelector)
        {
            foreach (var item in source)
            {
                dict.Add(item, keySelector(item));
            }
        }

        public static T FirstOfType<T, TBase>(this IEnumerable<TBase> source)
            where T : TBase
        {
            return source.OfType<T>().FirstOrDefault();
        }

        public static T FirstOfType<T, TBase>(this IEnumerable<TBase> source, Func<T, bool> predicate)
            where T : TBase
        {
            return source.OfType<T>().FirstOrDefault(predicate);
        }

        public static string AggregateString<T>(this IEnumerable<T> source, Func<T, string> formatter)
        {
            return source.Aggregate("", (result, next) => result + formatter.Invoke(next));
        }
    }
}