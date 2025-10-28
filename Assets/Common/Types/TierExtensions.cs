namespace Common.Types
{
    public static class TierExtensions
    {
        public static string ToDisplayString(this Tier tier)
        {
            return $"Tier {tier.ToRomanNumeral()}";
        }
    }
}