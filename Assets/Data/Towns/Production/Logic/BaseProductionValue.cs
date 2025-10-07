using Data.Configuration;
using Data.Modifiable;

namespace Data.Towns.Production.Logic
{
    public sealed class BaseProductionValue : BaseValueModifier
    {
        public BaseProductionValue(Good good) :
            base(GetProductionRate(good), $"Base production for Tier {GetTier(good)}")
        {
        }

        private static float GetProductionRate(Good good)
        {
            var tier = GetTier(good);
            return ConfigurationManager.Instance.ProducerConfig.GetProductionRate(tier);
        }

        private static Tier GetTier(Good good)
        {
            return ConfigurationManager.Instance.GoodsConfig.ConfigData[good].Tier;
        }
    }
}