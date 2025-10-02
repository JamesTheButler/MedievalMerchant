using Data.Modifiable;

namespace Data.Trade.Price
{
    public sealed class TownUpgradePriceModifier : BasePercentageModifier
    {
        public TownUpgradePriceModifier(float value) : base(value, "from Town Upgrades")
        {
        }
    }
}