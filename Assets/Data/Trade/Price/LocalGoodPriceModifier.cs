using Data.Modifiable;

namespace Data.Trade.Price
{
    public sealed class LocalGoodPriceModifier : BasePercentageModifier
    {
        public LocalGoodPriceModifier(float value) : base(value, "Local Good", "Good from local region.")
        {
        }
    }
}