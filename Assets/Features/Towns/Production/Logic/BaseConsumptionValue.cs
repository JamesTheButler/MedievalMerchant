using Common.Infrastructure;
using Common.Infrastructure.Modifiable;

namespace Features.Towns.Production.Logic
{
    public sealed class BaseConsumptionValue : BaseValueModifier
    {
        public BaseConsumptionValue() : base(GetProductionRate(), "Base Consumption") { }

        private static float GetProductionRate()
        {
            return ConfigurationManager.Configurations.ProducerConfig.ConsumptionRate;
        }
    }
}