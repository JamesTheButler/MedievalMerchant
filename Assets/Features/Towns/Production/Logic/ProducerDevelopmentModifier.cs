using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Types;

namespace Features.Towns.Production.Logic
{
    public sealed class ProducerDevelopmentModifier : FlatModifier
    {
        public int ProducerCount { get; }

        public ProducerDevelopmentModifier(float modifiedValue, int producerCount, Tier producerTier)
            : base(modifiedValue, GetDescription(producerCount, producerTier))
        {
            ProducerCount = producerCount;
        }

        private static string GetDescription(int producerCount, Tier producerTier)
        {
            var loc = ResourceManager.Instance.LocalizationResources.Town;
            return loc.ProducerDevelopmentModifier(producerCount, producerTier);
        }
    }
}