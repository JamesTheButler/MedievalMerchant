using Common.Modifiable;
using Common.Types;

namespace Features.Towns.Development.Logic
{
    public sealed class StoredGoodsDevelopmentModifier : FlatModifier
    {
        public int GoodCount { get; }

        public StoredGoodsDevelopmentModifier(float modifiedValue, int goodCount, Tier producerTier) :
            base(modifiedValue, $"{goodCount} {producerTier} foreign goods in storage.")
        {
            GoodCount = goodCount;
        }
    }
}