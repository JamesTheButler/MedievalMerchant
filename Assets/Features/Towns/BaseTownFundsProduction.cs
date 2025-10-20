using Common;
using Common.Modifiable;
using Common.Types;

namespace Features.Towns
{
    public sealed class BaseTownFundsProduction : BaseValueModifier
    {
        public BaseTownFundsProduction(float value, Tier townTier) : base(value, GetDescription(townTier)) { }

        public void Update(float value, Tier townTier)
        {
            Value.Value = value;
            Description.Value = GetDescription(townTier);
        }

        private static string GetDescription(Tier townTier)
        {
            return $"Base production for Tier {townTier.ToRomanNumeral()} town.";
        }
    }
}