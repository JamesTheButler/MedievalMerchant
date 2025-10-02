using Data.Modifiable;

namespace Data.Trade.Price
{
    public sealed class BasePriceModifier : BaseValueModifier
    {
        public BasePriceModifier(float value, Tier goodTier) : base(value,$"Price for {goodTier} good")
        {
        }
    }
}