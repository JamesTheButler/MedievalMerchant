namespace Common.Utility
{
    public static class StringExtensions
    {
        public static string Capitalize(this string str)
        {
            return str[..1].ToUpper() + str[1..];
        }
    }
}