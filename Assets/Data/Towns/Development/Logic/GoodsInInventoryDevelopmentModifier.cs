using Data.Modifiable;

namespace Data.Towns
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