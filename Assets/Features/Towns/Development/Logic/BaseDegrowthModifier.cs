using Common;
using Common.Modifiable;
using Common.Types;
using Features.Towns.Development.Config;

namespace Features.Towns.Development.Logic
{
    public sealed class BaseDegrowthModifier : FlatModifier
    {
        private readonly TownDevelopmentConfig _config;

        public BaseDegrowthModifier(Observable<Tier> townTierObservable) : base(0, string.Empty)
        {
            _config = ConfigurationManager.Configurations.TownDevelopmentConfig;

            townTierObservable.Observe(OnTierChanged);
        }

        private void OnTierChanged(Tier tier)
        {
            Value.Value = _config.BaseDegrowth[tier];
            Description.Value = $"Base degrowth for a {tier.ToDisplayString()} town";
        }
    }
}