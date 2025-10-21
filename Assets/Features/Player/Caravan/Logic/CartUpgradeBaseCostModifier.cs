using Common;
using Common.Modifiable;
using Features.Player.Caravan.Config;

namespace Features.Player.Caravan.Logic
{
    public sealed class CartUpgradeBaseCostModifier : BaseValueModifier
    {
        private readonly CaravanConfig _caravanConfig;

        public CartUpgradeBaseCostModifier(int level) : base(0, string.Empty)
        {
            _caravanConfig = ConfigurationManager.Instance.CaravanConfig;
            Update(level);
        }

        public void Update(int level)
        {
            Value.Value = _caravanConfig.GetUpgradeData(level).UpgradeCost;
            Description.Value = $"Base upgrade cost for Cart level {level}";
        }
    }
}