using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Common.Types;
using Common.Utility;
using Features.Localization.Data;
using Features.Towns.Development.Config;

namespace Features.Towns.Development.Logic
{
    public sealed class BaseDegrowthModifier : FlatModifier
    {
        private readonly TownDevelopmentConfig _config;
        private readonly TownLocalizationResources _loc;
        
        public BaseDegrowthModifier(Observable<Tier> townTierObservable) : base(0, string.Empty)
        {
            _config = ConfigurationManager.Configurations.TownDevelopmentConfig;
            _loc = ResourceManager.Instance.LocalizationResources.Town;

            townTierObservable.Observe(OnTierChanged);
        }

        private void OnTierChanged(Tier tier)
        {
            Value.Value = _config.BaseDegrowth[tier];
            var args = new { TierRoman = tier.ToRomanNumeral() };
            Description.Value = _loc.DevBaseRate.GetLocalizedString(args);
        }
    }
}