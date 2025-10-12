using Common.Modifiable;

namespace Features.Trade.Logic.Price
{
    public sealed class LocalGoodPriceModifier : BasePercentageModifier
    {
        public LocalGoodPriceModifier(float value) : base(value, "Good from local region.")
        {
        }
    }
}