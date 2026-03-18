using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Types;

namespace Features.Towns.Development.Logic
{
    public sealed class StoredGoodsDevelopmentModifier : FlatModifier
    {
        public int GoodCount { get; }

        public StoredGoodsDevelopmentModifier(float modifiedValue, int goodCount, Tier goodTier) :
            base(modifiedValue, GetDescription(goodCount, goodTier))
        {
            GoodCount = goodCount;
        }

        private static string GetDescription(int goodCount, Tier producerTier)
        {
            var loc = ResourceManager.Instance.LocalizationResources.Town;
            return loc.StoredGoodsDevelopmentModifier(goodCount, producerTier);
        }
    }
}