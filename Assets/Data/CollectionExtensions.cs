using System;
using System.Collections.Generic;
using Random = Unity.Mathematics.Random;

namespace Data
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Picks x unique random elements from a list.
        /// </summary>
        public static List<T> PickRandomUnique<T>(this IList<T> source, uint count)
        {
            if (count > source.Count)
            {
                count = (uint)source.Count;
            }

            var indices = new List<int>(source.Count);
            for (var i = 0; i < source.Count; i++)
            {
                indices.Add(i);
            }

            // Fisher–Yates shuffle, but only up to count
            for (var i = 0; i < count; i++)
            {
                var swapIndex = UnityEngine.Random.Range(i, indices.Count);
                (indices[i], indices[swapIndex]) = (indices[swapIndex], indices[i]);
            }

            // Collect results
            var result = new List<T>((int)count);
            for (var i = 0; i < count; i++)
                result.Add(source[indices[i]]);

            return result;
        }

        public static T PickRandom<T>(this IList<T> source)
        {
            if (source == null || source.Count == 0)
            {
                return default;
            }

            var index = UnityEngine.Random.Range(0, source.Count);
            return source[index];
        }
    }
}