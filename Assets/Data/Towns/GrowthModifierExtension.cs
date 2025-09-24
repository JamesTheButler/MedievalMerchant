namespace Data.Towns
{
    public static class GrowthModifierExtension
    {
        public static string ToDisplayString(this IGrowthModifier growthModifier)
        {
            var sign = growthModifier.Value > 0 ? "+" : "";
            return $"{sign}{growthModifier.Value}% .. {growthModifier.Description}";
        }
    }
}