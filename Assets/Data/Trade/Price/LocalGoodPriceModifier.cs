namespace Data.Trade.Price
{
    public sealed class LocalGoodPriceModifier : PriceModifier
    {
        public override float Value { get; }
        public override string Description { get; }

        public LocalGoodPriceModifier(float value)
        {
            Value = value;
            Description = "Good from local region.";
        }
    }
}