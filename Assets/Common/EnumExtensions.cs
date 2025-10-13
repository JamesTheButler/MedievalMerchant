using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Common
{
    public static class EnumExtensions
    {
        public static T GetRandom<T>() where T : Enum
        {
            var values = (T[])Enum.GetValues(typeof(T));
            var index = Random.Range(0, values.Length);
            return values[index];
        }

        public static TEnum AggregateFlags<TEnum>(this IEnumerable<TEnum> values)
            where TEnum : struct, Enum
        {
            var result = values.Aggregate(0, (current, value) => current | Convert.ToInt32(value));
            return (TEnum)Enum.ToObject(typeof(TEnum), result);
        }

        public static bool Intersects<TEnum>(this TEnum first, TEnum second)
            where TEnum : struct, Enum
        {
            return (Convert.ToInt64(first) & Convert.ToInt64(second)) != 0;
        }

        public static List<(T1 val1, T2 val2)> GetPermutations<T1, T2>()
            where T1 : Enum
            where T2 : Enum
        {
            var list = new List<(T1, T2)>();

            foreach (T1 v1 in Enum.GetValues(typeof(T1)))
            {
                foreach (T2 v2 in Enum.GetValues(typeof(T2)))
                {
                    list.Add((v1, v2));
                }
            }

            return list;
        }
    }
}