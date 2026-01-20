using Common.Infrastructure.Modifiable;

namespace Features.Trade.Logic.Price
{
    public sealed class LostInterestPriceModifier : BasePercentageModifier
    {
        public LostInterestPriceModifier(float value, string description) : base(value, description) { }
    }
}