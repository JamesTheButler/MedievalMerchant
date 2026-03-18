using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Types;
using Common.Utility;

namespace Features.Towns.Production.Logic
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
            var loc = ResourceManager.Instance.LocalizationResources.Town;
            var args = new { TierRoman = townTier.ToRomanNumeral() };
            return loc.FundsChangeBaseRate.GetLocalizedString(args);
        }
    }
}