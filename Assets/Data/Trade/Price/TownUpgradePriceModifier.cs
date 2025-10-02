namespace Data.Trade.Price
{
    public sealed class TownUpgradePriceModifier : PriceModifier
    {
        public override float Value { get; }
        public override string Description => "from Town Upgrades";

        public TownUpgradePriceModifier(float value)
        {
            Value = value;
        }
    }
}