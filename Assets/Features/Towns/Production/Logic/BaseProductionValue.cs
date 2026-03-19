using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Types;

namespace Features.Towns.Production.Logic
{
    public sealed class BaseProductionValue : BaseValueModifier
    {
        public BaseProductionValue(Good good) :
            base(
                GetProductionRate(good), GetDescription(good)) { }

        private static float GetProductionRate(Good good)
        {
            var tier = GetTier(good);
            return ConfigurationManager.Configurations.ProducerConfig.GetProductionRate(tier);
        }

        private static string GetDescription(Good good)
        {
            var tier = GetTier(good);
            return ResourceManager.Instance.LocalizationResources.Town.ProductionRateBase(tier);
        }

        private static Tier GetTier(Good good)
        {
            return ResourceManager.Instance.GoodResources.ResourceData[good].Tier;
        }
    }
}