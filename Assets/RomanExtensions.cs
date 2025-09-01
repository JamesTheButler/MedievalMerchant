using Data;
using JetBrains.Annotations;

[PublicAPI]
public static class RomanExtensions
{
    private const string Fallback = "???";

    private static readonly string[] Numerals =
    {
        "I",
        "II",
        "III",
        "IV",
        "V",
        "VI",
        "VII",
        "VIII",
        "IX",
        "X",
    };

    public static string ToRomanNumeral(this int input)
    {
        return input is < 1 or > 10 ? Fallback : Numerals[input - 1];
    }

    public static string ToRomanNumeral(this Tier tier)
    {
        return ((int)tier).ToRomanNumeral();
    }
}