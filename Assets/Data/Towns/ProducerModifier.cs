namespace Data.Towns
{
    public sealed record ProducerModifier : IGrowthModifier
    {
        public float Value { get; }
        public string Description { get; }

        public ProducerModifier(float modifiedValue, float producerCount, Tier producerTier)
        {
            Value = modifiedValue;
            Description = $"{producerCount} {producerTier} production buildings.";
        }
    }
}