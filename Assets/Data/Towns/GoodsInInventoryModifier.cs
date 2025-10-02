using Data.Modifiable;

namespace Data.Towns
{
    public sealed class GoodsInInventoryModifier : FlatModifier
    {
        public int GoodCount { get; }

        public GoodsInInventoryModifier(float modifiedValue, int goodCount, Tier producerTier) :
            base(modifiedValue, $"{goodCount} {producerTier} foreign goods in storage.")
        {
            GoodCount = goodCount;
        }
    }
}