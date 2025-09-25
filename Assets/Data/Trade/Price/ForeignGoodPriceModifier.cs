namespace Data.Trade.Price
{
    public sealed class ForeignGoodPriceModifier : PriceModifier
    {
        public override float Value { get; }
        public override string Description { get; }

        public ForeignGoodPriceModifier(float value)
        {
            Value = value;
            Description = "Good from foreign region.";
        }
    }
}