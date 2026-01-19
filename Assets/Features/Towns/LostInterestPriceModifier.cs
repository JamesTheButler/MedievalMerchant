using Common.Infrastructure.Modifiable;

namespace Features.Towns
{
    public sealed class LostInterestPriceModifier : BasePercentageModifier
    {
        public LostInterestPriceModifier(float value, string description) : base(value, description) { }
    }
}