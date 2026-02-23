namespace Unity.Localization.Roslyn;

public static class StringExtensions
{
    public static string TrimEnd(this string str, string end)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        return str.EndsWith(end) ? str.Substring(0, str.Length - end.Length) : str;
    }
}