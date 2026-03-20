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

        public static string TrimStart(this string str, string trim)
        {
            if (string.IsNullOrEmpty(str) || !str.StartsWith(trim))
                return str;

            return str[trim.Length..];
        }
        
    }
}