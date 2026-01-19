using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Types;
using Features.Goods.Config;

namespace Features.Trade.Logic.Price
{
    /// <summary>
    /// Price modifier based on the availability of a good in that town.
    /// </summary>
    public sealed class AvailabilityPriceModifier : BasePercentageModifier
    {
        private readonly PriceModifierConfig _config = ConfigurationManager.Configurations.PriceModifierConfig;
        private readonly AvailabilityResources _resources = ResourceManager.Instance.AvailabilityResources;

        public AvailabilityPriceModifier(Availability availability) : base(0, string.Empty)
        {
            Update(availability);
        }

        public void Update(Availability availability)
        {
            Value.Value = _config.AvailabilityConfigData[availability].PriceMultiplier;
            Description.Value = GetDescription(availability);
        }

        private string GetDescription(Availability availability)
        {
            var availabilityString = _resources.Resources[availability].DisplayString;
            return $"Availability: {availabilityString}";
        }
    }
}