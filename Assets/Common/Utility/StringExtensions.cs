using System.Collections.Generic;
using System.Linq;

namespace Common.Utility
{
    public static class StringExtensions
    {
        public static string Capitalize(this string str)
        {
            return str[..1].ToUpper() + str[1..];
        }

        public static string JoinWithAnd(this IEnumerable<string> strings)
        {
            var array = strings.ToArray();
            var count = array.Length;

            return count switch
            {
                0 => string.Empty,
                1 => array[0],
                _ => string.Join(", ", array[..^1]) + " and " + array[^1]
            };
        }
    }
}