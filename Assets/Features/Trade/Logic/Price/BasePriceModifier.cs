using Common.Modifiable;
using Common.Types;

namespace Features.Trade.Logic.Price
{
    public sealed class BasePriceModifier : BaseValueModifier
    {
        public BasePriceModifier(float value, Tier goodTier) : base(value,$"Price for {goodTier} good")
        {
        }
    }
}