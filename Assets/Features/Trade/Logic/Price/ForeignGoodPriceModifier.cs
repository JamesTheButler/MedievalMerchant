using Common.Modifiable;

namespace Features.Trade.Logic.Price
{
    public sealed class ForeignGoodPriceModifier : BasePercentageModifier
    {
        public ForeignGoodPriceModifier(float value) : base(value, "Good from foreign region.")
        {
        }
    }
}