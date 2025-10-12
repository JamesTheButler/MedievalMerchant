using Common.Modifiable;

namespace Features.Trade.Logic.Price
{
    public sealed class TownUpgradePriceModifier : BasePercentageModifier
    {
        public TownUpgradePriceModifier(float value) : base(value, "from Town Upgrades")
        {
        }
    }
}