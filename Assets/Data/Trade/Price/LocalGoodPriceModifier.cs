using Data.Modifiable;

namespace Data.Trade.Price
{
    public sealed class LocalGoodPriceModifier : BasePercentageModifier
    {
        public LocalGoodPriceModifier(float value) : base(value, "Good from local region.")
        {
        }
    }
}