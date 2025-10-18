using Common.Modifiable;
using Common.Types;

namespace Features.Towns.Production.Logic
{
    public sealed class ProducerDevelopmentModifier : FlatModifier
    {
        public int ProducerCount { get; }

        public ProducerDevelopmentModifier(float modifiedValue, int producerCount, Tier producerTier)
            : base(modifiedValue, $"{producerCount} {producerTier} production buildings.")
        {
            ProducerCount = producerCount;
        }
    }
}