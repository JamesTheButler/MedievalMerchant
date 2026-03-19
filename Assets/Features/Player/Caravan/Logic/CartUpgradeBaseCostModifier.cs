using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Features.Localization.Data;
using Features.Player.Caravan.Config;

namespace Features.Player.Caravan.Logic
{
    public sealed class CartUpgradeBaseCostModifier : BaseValueModifier
    {
        private readonly CaravanConfig _caravanConfig;
        private readonly PlayerLocalizationResources _loc;

        public CartUpgradeBaseCostModifier(int level) : base(0, string.Empty)
        {
            _caravanConfig = ConfigurationManager.Configurations.CaravanConfig;
            _loc = ResourceManager.Instance.LocalizationResources.Player;
            Update(level);
        }

        public void Update(int level)
        {
            Value.Value = _caravanConfig.GetUpgradeData(level).UpgradeCost;
            Description.Value = _loc.UpgradeCostBase(level);
        }
    }
}