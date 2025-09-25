using Data.Configuration;

namespace Data.Trade.Price
{
    public sealed class AvailabilityPriceModifier : PriceModifier
    {
        public override float Value { get; }
        public override string Description { get; }

        public AvailabilityPriceModifier(float value, Availability availability)
        {
            var availabilityConfig = ConfigurationManager.Instance.AvailabilityConfig;
            var availabilityString = availabilityConfig.ConfigData[availability].DisplayString;

            Value = value;
            Description = $"Availability: {availabilityString}";
        }
    }
}