using Common.Infrastructure.Modifiable;

namespace Features.Levels.GameModifiers.Logic
{
    public sealed class EffectPercentModifier : BasePercentageModifier
    {
        public EffectPercentModifier(float value, EffectOrigin origin) : base(value, GetDescription(origin)) { }

        private static string GetDescription(EffectOrigin origin)
        {
            // should come out at "Event: Heavy Rain" or "Level Modifier: Golden Times"
            return $"{origin.Type}: {origin.Title}";
        }
    }
}