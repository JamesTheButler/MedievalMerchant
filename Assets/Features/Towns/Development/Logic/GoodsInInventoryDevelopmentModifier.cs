using Common.Modifiable;
using Common.Types;

namespace Features.Towns.Development.Logic
{
    public sealed class GoodsInInventoryDevelopmentModifier : FlatModifier
    {
        public int GoodCount { get; }

        public GoodsInInventoryDevelopmentModifier(float modifiedValue, int goodCount, Tier producerTier) :
            base(modifiedValue, $"{goodCount} {producerTier} foreign goods in storage.")
        {
            GoodCount = goodCount;
        }
    }
}