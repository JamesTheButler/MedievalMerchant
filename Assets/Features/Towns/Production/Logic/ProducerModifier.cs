using Common.Modifiable;
using Common.Types;

namespace Features.Towns.Production.Logic
{
    public sealed class ProducerModifier : FlatModifier
    {
        public int ProducerCount { get; }

        public ProducerModifier(float modifiedValue, int producerCount, Tier producerTier)
            : base(modifiedValue, $"{producerCount} {producerTier} production buildings.")
        {
            ProducerCount = producerCount;
        }
    }
}