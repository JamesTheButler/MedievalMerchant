using Common;
using Common.Modifiable;
using Common.Types;

namespace Features.Trade.Logic.Price
{
    public sealed class AvailabilityPriceModifier : BasePercentageModifier
    {
        public AvailabilityPriceModifier(float value, Availability availability)
            : base(value, GetDescription(availability))
        {
        }

        private static string GetDescription(Availability availability)
        {
            var availabilityConfig = ConfigurationManager.Instance.AvailabilityConfig;
            var availabilityString = availabilityConfig.ConfigData[availability].DisplayString;
            return $"Availability: {availabilityString}";
        }
    }
}