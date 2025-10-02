using Data.Modifiable;

namespace Data.Trade.Price
{
    public sealed class ForeignGoodPriceModifier : BasePercentageModifier
    {
        public ForeignGoodPriceModifier(float value) : base(value, "Good from foreign region.")
        {
        }
    }
}