using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common
{
    public static class CollectionExtensions
    {
        public static T GetRandom<T>(this IList<T> source)
        {
            if (source == null || source.Count == 0)
            {
                return default;
            }

            var index = Random.Range(0, source.Count);
            return source[index];
        }

        public static string PrettyPrint<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            return "{\n" + string.Join("\n", dict.Select(kvp => $"{kvp.Key}: {kvp.Value}")) + "\n}";
        }
    }
}