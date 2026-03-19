using Common.Infrastructure;
using Common.Infrastructure.Modifiable;

namespace Features.Towns.Production.Logic
{
    public sealed class BaseConsumptionValue : BaseValueModifier
    {
        public BaseConsumptionValue() : base(
            GetProductionRate(),
            GetDescription()) { }

        private static string GetDescription()
        {
            return ResourceManager.Instance.LocalizationResources.Town.ConsumptionRateBase.GetLocalizedString();
        }

        private static float GetProductionRate()
        {
            return ConfigurationManager.Configurations.ProducerConfig.ConsumptionRate;
        }
    }
}