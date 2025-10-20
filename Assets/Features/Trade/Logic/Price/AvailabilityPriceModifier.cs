using Common;
using Common.Modifiable;
using Common.Types;
using Features.Goods.Config;

namespace Features.Trade.Logic.Price
{
    /// <summary>
    /// Price modifier based on the availability of a good in that town.
    /// </summary>
    public sealed class AvailabilityPriceModifier : BasePercentageModifier
    {
        private readonly AvailabilityConfig _config = ConfigurationManager.Instance.AvailabilityConfig;

        public AvailabilityPriceModifier(Availability availability)
            : base(0, string.Empty)
        {
            Update(availability);
        }

        public void Update(Availability availability)
        {
            Value.Value = _config.ConfigData[availability].PriceMultiplier;
            Description.Value = GetDescription(availability);
        }

        private string GetDescription(Availability availability)
        {
            var availabilityString = _config.ConfigData[availability].DisplayString;
            return $"Availability: {availabilityString}";
        }
    }
}